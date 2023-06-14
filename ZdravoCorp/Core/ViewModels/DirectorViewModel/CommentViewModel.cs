using ZdravoCorp.Core.Models.Surveys;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

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