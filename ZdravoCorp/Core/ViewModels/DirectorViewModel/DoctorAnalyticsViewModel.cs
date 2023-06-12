using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.ServayServices;
using ZdravoCorp.View.DirectorView;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DoctorAnalyticsViewModel : ViewModelBase
{
     private ObservableCollection<DoctorSurveyViewModel> _surveys;
    private ObservableCollection<CommentViewModel> _comments;
    private ObservableCollection<GradesViewModel> _grades;
    private ObservableCollection<DoctorGradePosition> _bestDoctors;
    private ObservableCollection<DoctorGradePosition> _worstDoctors;
    private ISurveyService _surveyService;
    private IDoctorService _doctorService;
    public DoctorSurveyViewModel? SelectedSurvey { get; set; }
    public ICommand ViewSurveyCommand { get; set; }

    public IEnumerable<DoctorSurveyViewModel> Surveys
    {
        get => _surveys;
        set
        {
            _surveys = new ObservableCollection<DoctorSurveyViewModel>(value);
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

    public IEnumerable<DoctorGradePosition> BestDoctors
    {
        get => _bestDoctors;
        set
        {
            _bestDoctors = new ObservableCollection<DoctorGradePosition>(value);
            OnPropertyChanged();
        }
    }
    public IEnumerable<DoctorGradePosition> WorstDoctors
    {
        get => _worstDoctors;
        set
        {
            _worstDoctors = new ObservableCollection<DoctorGradePosition>(value);
            OnPropertyChanged();
        }
    }
    public DoctorAnalyticsViewModel(ISurveyService surveyService, IDoctorService doctorService)
    {
        _surveyService = surveyService;
        _doctorService = doctorService;
        _grades = new ObservableCollection<GradesViewModel>();
        _surveys = new ObservableCollection<DoctorSurveyViewModel>();
        _comments = new ObservableCollection<CommentViewModel>();
        _bestDoctors = new ObservableCollection<DoctorGradePosition>();
        _worstDoctors = new ObservableCollection<DoctorGradePosition>();
        ViewSurveyCommand = new DelegateCommand(o => ViewSurvey(), o => CanViewSurvey());
        PopulateTables();
    }

    private bool CanViewSurvey()
    {
        return SelectedSurvey != null;
    }
    private void ViewSurvey()
    {
        var vm = new DisplayDoctorSurveyViewModel(_surveyService.FindExistingDoctorSurvey(SelectedSurvey.Doctor, SelectedSurvey.Patient));
        var window = new DisplayDoctorSurveyView()
        {
            DataContext = vm
               
        };
        vm.OnRequestClose += (s, e) => window.Close();
        window.Show();
    }

    private void PopulateTables()
    {
        foreach (var survey in _surveyService.GetAllDoctorSurveys()!)
        {
            _surveys.Add(new DoctorSurveyViewModel(survey));
            _comments.Add(new CommentViewModel(survey.PatientEmail, survey.Comment));
        }

        foreach (var doctor in _doctorService.GetAll())
        {
            _grades.Add(new GradesViewModel(_surveyService.GetGradesForDoctor(doctor.Email), doctor.FullName));
        }

        var sortedGrades = _surveyService.GetAllDoctorsWithGrades().OrderBy(x=>x.Value).ToList();
        var numberOfDoctors = sortedGrades.Count < 3 ? sortedGrades.Count : 3; 
        
        for (int i = 0; i < numberOfDoctors; i++)
        {
            _worstDoctors.Add(new DoctorGradePosition((i+1).ToString() + ".", sortedGrades[i].Key));
            _bestDoctors.Add(new DoctorGradePosition((i+1).ToString() + ".", sortedGrades[sortedGrades.Count - i - 1].Key));
        }


    }
    
    
}