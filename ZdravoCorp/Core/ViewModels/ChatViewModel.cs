using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Discord;
using Discord.WebSocket;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.ViewModels;

public class ChatViewModel : ViewModelBase
{
    private DiscordSocketClient _discordClient;
    private ObservableCollection<MessageViewModel> _messages;
    private string _inputText;

    public ChatViewModel()
    {
        var config = new DiscordSocketConfig
        {
            // Other configurations
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };
        _discordClient = new DiscordSocketClient(config);
        _messages = new ObservableCollection<MessageViewModel>();

        // Configure Discord bot events
        //_discordClient.MessageReceived += OnMessageReceived;
        _discordClient.MessageReceived += async (message) => await OnMessageReceived(message);


        // Connect and start the Discord bot
        _discordClient.LoginAsync(TokenType.Bot, "MTExNDkwMTgyNTUyMzU0ODIzMA.Gr750P.7_4fejNLE_yrtCKRSCuIzvYRG-bBGC7Q45Ai7A").Wait();
        _discordClient.StartAsync().Wait();

        SendCommand = new DelegateCommand(o=>SendMessage());
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
        await Task.Delay(2000); // Introduce a delay of 2 seconds (adjust as needed)
        await LoadPreviousMessagesAsync();
    }

    private async Task LoadPreviousMessagesAsync()
    {
        await LoadPreviousMessages();
    }

    private async Task LoadPreviousMessages()
    {
        // Get the Discord guild and channel objects based on your guild and channel IDs
        var channel = _discordClient.GetGuild(1114665077916835952).GetTextChannel(1114665079158341764);
        //var channel1 = guild.GetTextChannel(1114665079158341764);

        // Fetch the previous messages from the channel
        var messages = await channel.GetMessagesAsync().FlattenAsync();

        // Convert the Discord messages to MessageViewModels and add them to the Messages collection
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

    //private void OnMessageReceived(SocketMessage message)
    //{
    //    // Add the received message to the chat window
    //    App.Current.Dispatcher.Invoke(() =>
    //    {
    //        var newMessage = new MessageViewModel
    //        {
    //            Sender = message.Author.Username,
    //            Content = message.Content
    //        };

    //        Messages.Add(newMessage);
    //    });
    //}

    private void SendMessage()
    {
        //LoadPreviousMessagesAsync();

        // Send the message to the Discord channel
        _discordClient.GetGuild(1114665077916835952)
            .GetTextChannel(1114665079158341764)
            .SendMessageAsync(InputText);

        // Clear the input field
        InputText = "";
    }
    public class MessageViewModel
    {
        public string Sender { get; set; }
        public string Content { get; set; }
    }
}
