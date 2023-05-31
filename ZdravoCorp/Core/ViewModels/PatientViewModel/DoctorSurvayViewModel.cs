using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Autofac;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Survays;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Services.ServayServices;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class DoctorSurvayViewModel : ViewModelBase
{
    private ISurvayService _survayService;
    private Doctor? _doctor;
    private string _patientEmail;
    private string _selectedGrade;
    private bool _yesChecked;
    private bool _noChecked;
    private string _comment = "";

    public ICommand SubmitSurvayCommand { get; set; }
    public DoctorSurvayViewModel(Doctor? doctor, string patientEmail)
    {
        _survayService = Injector.Container.Resolve<ISurvayService>();
        _doctor = doctor;
        _patientEmail = patientEmail;
        PossibleGrades = new HashSet<string> { "1", "2", "3", "4", "5" };
        _selectedGrade = "5";
        _yesChecked = true;

        SubmitSurvayCommand = new DelegateCommand(o => SubmitSurvayComm());
    }

    public string DoctorName => _doctor.FullName;
    public string DoctorSpecialization => _doctor.Specialization.ToString();
    public HashSet<string> PossibleGrades { get; set; }
    public string SelectedGrade
    {
        get => _selectedGrade;
        set
        {
            _selectedGrade = value;
            OnPropertyChanged();
        }
    }

    public bool YesChecked
    {
        get => _yesChecked;
        set
        {
            _yesChecked = value;
            OnPropertyChanged();
        }
    }
    public bool NoChecked
    {
        get => _noChecked;
        set
        {
            _noChecked = value;
            OnPropertyChanged();
        }
    }
    public string Comment
    {
        get => _comment;
        set
        {
            _comment = value;
            OnPropertyChanged();
        }
    }

    private void SubmitSurvayComm()
    {
        var survay = new DoctorSurvayDTO(IDGenerator.GetId(), _doctor.Email, _patientEmail,
            int.Parse(SelectedGrade), YesChecked, Comment);
        _survayService.AddDoctorSuvay(survay);
        MessageBox.Show("Survay created successfully", "Success", MessageBoxButton.OK);

    }



}