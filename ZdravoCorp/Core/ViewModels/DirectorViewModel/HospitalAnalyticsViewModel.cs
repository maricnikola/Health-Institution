using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Services.ServayServices;
using ZdravoCorp.View.DirectorView;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class HospitalAnalyticsViewModel : ViewModelBase
{

    private ObservableCollection<HospitalSurveyViewModel> _surveys;
    private ObservableCollection<CommentViewModel> _comments;
    private ObservableCollection<GradesViewModel> _grades;
    private ISurveyService _surveyService;
    public HospitalSurveyViewModel? SelectedSurvey { get; set; }
    public ICommand ViewSurveyCommand { get; set; }

    public IEnumerable<HospitalSurveyViewModel> Surveys
    {
        get => _surveys;
        set
        {
            _surveys = new ObservableCollection<HospitalSurveyViewModel>(value);
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
    public HospitalAnalyticsViewModel(ISurveyService surveyService)
    {
        _surveyService = surveyService;
        _grades = new ObservableCollection<GradesViewModel>();
        _surveys = new ObservableCollection<HospitalSurveyViewModel>();
        _comments = new ObservableCollection<CommentViewModel>();
        ViewSurveyCommand = new DelegateCommand(o => ViewSurvey(), o => CanViewSurvey());
        PopulateTables();
    }

    private bool CanViewSurvey()
    {
        return SelectedSurvey != null;
    }
    private void ViewSurvey()
    {
        var vm = new DisplayHospitalSurveyViewModel(_surveyService.FindHospitalSurveyForPatient(SelectedSurvey.User));
        var window = new DisplayHospitalSurveyView()
        {
            DataContext = vm
               
        };
        vm.OnRequestClose += (s, e) => window.Close();
        window.Show();
    }

    private void PopulateTables()
    {
        foreach (var survey in _surveyService.GetAllHospitalSurveys()!)
        {
            _surveys.Add(new HospitalSurveyViewModel(survey));
            _comments.Add(new CommentViewModel(survey));
        }
        _grades.Add(new GradesViewModel(_surveyService.GetHospitalServiceGrades(), "Service:"));
        _grades.Add(new GradesViewModel(_surveyService.GetHospitalHygieneGrades(), "Hygiene:"));
        _grades.Add(new GradesViewModel(_surveyService.GetHospitalOverallGrades(), "Overall:"));

        
    }
    
    
}