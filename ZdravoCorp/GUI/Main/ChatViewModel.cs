using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Discord;
using Discord.WebSocket;

namespace ZdravoCorp.GUI.Main;

public class ChatViewModel : ViewModelBase
{
    private DiscordSocketClient _discordClient;
    private ObservableCollection<MessageViewModel> _messages;
    private string _inputText;

    public ChatViewModel(string clientToken)
    {
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };
        _discordClient = new DiscordSocketClient(config);
        _messages = new ObservableCollection<MessageViewModel>();

        //_discordClient.MessageReceived += OnMessageReceived;
        _discordClient.MessageReceived += async (message) => await OnMessageReceived(message);


        _discordClient.LoginAsync(TokenType.Bot, clientToken).Wait();
        _discordClient.StartAsync().Wait();

        SendCommand = new SendMessageCommand(this, _discordClient);
        //SendCommand = new DelegateCommand(o=>SendMessage());
        InitializeAsync();
    }

    private async Task OnMessageReceived(SocketMessage message)
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            var newMessage = new MessageViewModel
            {
                Sender = message.Author.Username,
                Content = message.Content
            };

            _messages.Add(newMessage);
        });
    }
    
    private async void InitializeAsync()
    {
        var counter = 0;
        while (counter <= 20)
        {
            try
            {
                await Task.Delay(1000);
                await LoadPreviousMessagesAsync();
                break;
            }
            catch
            {
                counter++;
            }
        }
        if (counter==21)
            MessageBox.Show("connection with discord server failed, check your connection", "Error", MessageBoxButton.OK);

    }

    private async Task LoadPreviousMessagesAsync()
    {
        await LoadPreviousMessages();
    }

    private async Task LoadPreviousMessages()
    {
        var channel = _discordClient.GetGuild(1114665077916835952).GetTextChannel(1114665079158341764);
        //var channel1 = guild.GetTextChannel(1114665079158341764);

        var messages = await channel.GetMessagesAsync().FlattenAsync();

        foreach (var message in messages)
        {
            var messageViewModel = new MessageViewModel
            {
                Sender = message.Author.Username,
                Content = message.Content
            };

            _messages.Insert(0, messageViewModel);
        }
    }

    public ObservableCollection<MessageViewModel> Messages
    {
        get => _messages;
        set
        {
            _messages = value;
            OnPropertyChanged();
        }
    }

    public string InputText
    {
        get => _inputText;
        set
        {
            _inputText = value;
            OnPropertyChanged();
        }
    }

    public ICommand SendCommand { get; }

    private void SendMessage()
    {
        _discordClient.GetGuild(1114665077916835952)
            .GetTextChannel(1114665079158341764)
            .SendMessageAsync(InputText);
        InputText = "";
    }
}
