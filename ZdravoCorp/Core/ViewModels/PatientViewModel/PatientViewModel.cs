using Quartz.Impl;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class PatientViewModel : ViewModelBase
{
    private object _currentView;
    private ScheduleRepository _scheduleRepository;
    private DoctorRepository _doctorRepository;
    private MedicalRecordRepository _medicalRecordRepository;
    private Patient _patient;
    
    public ICommand LoadAppointmentsCommand { get; set; }
    public ICommand LoadMedicalRecordCommand { get; set; }
    public ICommand LoadOldAppointmentsCommand { get; set; }


    public object CurrentView
    {
        get { return _currentView; }
        set
        {
            _currentView = value;
            OnPropertyChanged("CurrentView");
        }
    }
    
    public PatientViewModel(Patient patient,RepositoryManager _repositoryManager)
    {
        _scheduleRepository = _repositoryManager.ScheduleRepository;
        _doctorRepository = _repositoryManager.DoctorRepository;
        _patient = patient;
        _medicalRecordRepository = _repositoryManager.MedicalRecordRepository;
        LoadAppointmentsCommand = new DelegateCommand(o => LoadAppointments());
        LoadMedicalRecordCommand = new DelegateCommand(o => LoadMedicalRecord());
        LoadOldAppointmentsCommand = new DelegateCommand(o => LoadOldAppointments());
        _currentView = new AppointmentTableViewModel(_scheduleRepository, _doctorRepository, _patient);
    }

    public void LoadAppointments()
    {
        CurrentView = new AppointmentTableViewModel(_scheduleRepository, _doctorRepository, _patient);
    }

    public void LoadMedicalRecord()
    {
        CurrentView = new MedicalRecordViewModel(_medicalRecordRepository.GetById(_patient.Email), _medicalRecordRepository);
    }

    public void LoadOldAppointments()
    {
        CurrentView = new OldAppointmentsViewModel(_scheduleRepository, _doctorRepository, _patient);
    }

}