using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Loader;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.Core.ViewModels.DoctorViewModels;
using ZdravoCorp.View;
using ZdravoCorp.View.DoctorView;
using MedicalRecordView = ZdravoCorp.View.MedicalRecordView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class AppointmentShowViewModel : ViewModelBase
{
    private readonly ObservableCollection<AppointmentViewModel> _appointments;
    private readonly ObservableCollection<MedicalRecordViewModel> _medicalRecords;
    private ScheduleRepository _scheduleRepository;
    private DoctorRepository _doctorRepository;
    private PatientRepository _patientRepository;
    private MedicalRecordRepository _medicalRecordRepository;
    private Doctor _doctor;
    private int counterViews;

    public ObservableCollection<AppointmentViewModel> Appointments => _appointments;

    public AppointmentViewModel SelectedAppointments { get; set; }
    public ICommand ChangeAppointmentCommand { get; }
    public ICommand AddAppointmentCommand { get; }
    public ICommand CancelAppointmentCommand { get; }
    public ICommand SearchAppointmentCommand { get; }
    public ICommand ViewMedicalRecordCommand { get; }

    public AppointmentShowViewModel(User user, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository, PatientRepository patientRepository,MedicalRecordRepository medicalRecordRepository)
    {
        counterViews = 0;
        _patientRepository = patientRepository;
        _scheduleRepository = scheduleRepository;

        _doctorRepository = doctorRepository;
        //_doctor = _doctorRepository.GetDoctorByEmail(user.Email);

        //List<Appointment> appointments = _scheduleRepository.GetDoctorAppointments(_doctor.Email);
        _medicalRecordRepository = medicalRecordRepository;

        _appointments = new ObservableCollection<AppointmentViewModel>();

        //foreach (Appointment appointment in appointments)
        //{
        //    _appointments.Add(new AppointmentViewModel(appointment));
        //}

        AddAppointmentCommand = new DelegateCommand(o => OpenAddDialog());
        ChangeAppointmentCommand = new DelegateCommand(o => OpenChangeDialog());
        CancelAppointmentCommand = new DelegateCommand(o => CancelAppointment());
        SearchAppointmentCommand = new DelegateCommand(o => SearchAppointments());
        ViewMedicalRecordCommand = new DelegateCommand(o => ShowMedicalRecord());


    }

    public void OpenAddDialog()
    {
        var addAp = new AddAppointmentView() { DataContext = new AddAppointmentViewModel(_scheduleRepository, _doctorRepository, _appointments, _patientRepository, _doctor, _medicalRecordRepository, _dateAppointment) };
        addAp.Show();
    }

    public void OpenChangeDialog()
    {
        AppointmentViewModel appointment = SelectedAppointments;
        if (appointment != null)
        {
            string patientMail = appointment.PatientMail;
            Patient patient = _patientRepository.GetPatientByEmail(patientMail);
            var changeAp = new DrChangeAppointmentView() { DataContext = new DrChangeAppointmentViewModel(SelectedAppointments, _scheduleRepository, _doctorRepository, _appointments, _patientRepository, _doctor, patient, appointment, _dateAppointment) };
            changeAp.Show();

        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void CancelAppointment()
    {
        AppointmentViewModel selectedAppointment = SelectedAppointments;
        if (selectedAppointment != null)
        {
            //Appointment appointment = _controller.GetAppointmentById(selectedAppointment.Id);
            //_controller.CancelAppointment(appointment);
            Appointments.Remove(GetById(selectedAppointment.Id, Appointments));

        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }

    }

    public AppointmentViewModel GetById(int id, ObservableCollection<AppointmentViewModel> Appointments)
    {
        foreach (var appointmentViewModel in Appointments)
        {
            if (appointmentViewModel.Id == id) return appointmentViewModel;
        }
        return null;
    }

    private DateTime _dateAppointment = DateTime.Now + TimeSpan.FromHours(1);
    public DateTime DateAppointment
    {
        get
        {
            return _dateAppointment;
        }
        set
        {
            _dateAppointment = value;
            OnPropertyChanged(nameof(DateAppointment));
        }
    }

    public void SearchAppointments()
    {
        List<Appointment> showAppointments = _scheduleRepository.GetAppointmentsForShow(_dateAppointment);
        Appointments.Clear();
        foreach (Appointment appointment in showAppointments)
        {
            Appointments.Add(new AppointmentViewModel(appointment));
        }

    }

    public void ShowMedicalRecord()
    {
        AppointmentViewModel appointment = SelectedAppointments;
        if (appointment != null)
        {
            MedicalRecord medicalR = _medicalRecordRepository.GetById(appointment.PatientMail);
            MedicalRecordView window = new MedicalRecordView() { DataContext = new MedicalRecordViewModel(medicalR) };
            window.Show();

        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }

    }



}
