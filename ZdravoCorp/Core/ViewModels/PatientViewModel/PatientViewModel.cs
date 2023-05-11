using Quartz.Impl;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class PatientViewModel : ViewModelBase
{
    private object _currentView;
    private List<Appointment> _appointments;
    private ScheduleRepository _scheduleRepository;
    private DoctorRepository _doctorRepository;
    private MedicalRecordRepository _medicalRecordRepository;
    private Patient _patient;
    
    public ICommand LoadAppointmentsCommand { get; set; }
    public ICommand LoadMedicalRecordCommand { get; set; }


    public object CurrentView
    {
        get
        {
            return _currentView;
        }
        set
        {
            _currentView = value;
            OnPropertyChanged("CurrentView");
        }
    }
    
    public PatientViewModel(List<Appointment> appointments, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository, Patient patient, MedicalRecordRepository medicalRecordRepository)
    {
        _appointments=appointments;
        _scheduleRepository = scheduleRepository;
        _doctorRepository = doctorRepository;
        _patient = patient;
        _medicalRecordRepository = medicalRecordRepository;
        LoadAppointmentsCommand = new DelegateCommand(o => LoadAppointments());
        LoadMedicalRecordCommand = new DelegateCommand(o => LoadMedicalRecord());
        _currentView = new AppointmentTableViewModel(_appointments, scheduleRepository, _doctorRepository, _patient);
    }

    public void LoadAppointments()
    {
        CurrentView = new AppointmentTableViewModel(_appointments, _scheduleRepository, _doctorRepository, _patient);
    }

    public void LoadMedicalRecord()
    {
        CurrentView = new MedicalRecordViewModel(_medicalRecordRepository.GetById(_patient.Email), _medicalRecordRepository);
    }

}