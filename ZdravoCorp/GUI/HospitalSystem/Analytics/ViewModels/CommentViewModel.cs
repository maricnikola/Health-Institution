using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;

public class CommentViewModel : ViewModelBase
{
    public CommentViewModel(string user, string text)
    {
        User = user;
        Text = text;
    }
    public string User { get; }
    public string Text { get; }
}