using System;
using System.Windows.Input;
using Discord.WebSocket;

namespace ZdravoCorp.GUI.Main;

public class SendMessageCommand : CommandBase
{
    private ChatViewModel _chatViewModel;
    private DiscordSocketClient _client;

    public SendMessageCommand(ChatViewModel chatViewModel, DiscordSocketClient client)
    {
        _chatViewModel = chatViewModel;
        _client = client;
    }

    public override bool CanExecute(object? parameter)
    {
        return _chatViewModel.InputText != "";
    }

    public override void Execute(object? parameter)
    {
        _client.GetGuild(1114665077916835952)
            .GetTextChannel(1114665079158341764)
            .SendMessageAsync(_chatViewModel.InputText);
        _chatViewModel.InputText = "";
    }

    public event EventHandler? CanExecuteChanged;
}