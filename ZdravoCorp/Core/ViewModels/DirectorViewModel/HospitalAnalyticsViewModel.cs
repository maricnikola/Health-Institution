using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class HospitalAnalyticsViewModel : ViewModelBase
{

    private ObservableCollection<SurveyViewModel> _surveys;
    private ObservableCollection<CommentViewModel> _comments;
    private ObservableCollection<GradesViewModel> _grades;

    public IEnumerable<SurveyViewModel> Surveys
    {
        get => _surveys;
        set
        {
            _surveys = new ObservableCollection<SurveyViewModel>(value);
            OnPropertyChanged();
        }
    }

    public IEnumerable<CommentViewModel> Comments
    {
        get => _comments;
        set
        {
            _comments = new ObservableCollection<CommentViewModel>(value);
            OnPropertyChanged();
        }
    }

    public IEnumerable<GradesViewModel> Grades
    {
        get => _grades;
        set
        {
            _grades = new ObservableCollection<GradesViewModel>(value);
            OnPropertyChanged();
        }
    }
    public HospitalAnalyticsViewModel()
    {
        _grades = new ObservableCollection<GradesViewModel>();
        _grades.Add(new GradesViewModel(1, 2,3 ,4 ,5 ,"3.5", "Service:"));
        _grades.Add(new GradesViewModel(1, 2,3 ,4 ,5 ,"1.55", "Hygiene:"));
        _grades.Add(new GradesViewModel(1, 2,3 ,4 ,5 ,"4.55", "Overall:"));
    }
}