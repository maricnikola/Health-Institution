using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.View;
using ZdravoCorp.View.PatientV;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class AppointmentTableViewModel : ViewModelBase
{
    private readonly ObservableCollection<AppointmentViewModel> _appointments;

    public ObservableCollection<AppointmentViewModel> Appointments => _appointments;
    public AppointmentViewModel SelectedAppointment { get; set; }
    private ScheduleRepository _controller;
    private DoctorRepository _doctorRepository;
    private Patient _patient;

    public ICommand NewAppointmentCommand { get; set; }
    public ICommand ChangeAppointmentCommand { get; set; }
    public ICommand CancelAppointmentCommand { get; set; }

    public AppointmentTableViewModel()
    {
    }

    public AppointmentTableViewModel(List<Appointment> appointments, ScheduleRepository scheduleRepository,
        DoctorRepository doctorRepository, Patient patient)
    {
        _patient = patient;
        _controller = scheduleRepository;
        _appointments = new ObservableCollection<AppointmentViewModel>();
        _doctorRepository = doctorRepository;
        foreach (var appointment in appointments)
        {
            _appointments.Add(new AppointmentViewModel(appointment));
        }

        NewAppointmentCommand = new DelegateCommand(o => NewAppointment());
        ChangeAppointmentCommand = new DelegateCommand(o => ChangeAppointmentComm());
        CancelAppointmentCommand = new DelegateCommand(o => CancelAppointmentComm());
    }

    private void ChangeAppointmentComm()
    {
        AppointmentViewModel selectedAppointment = SelectedAppointment;
        if (selectedAppointment != null)
        {
            Appointment appointment = _controller.GetAppointmentById(selectedAppointment.Id);
            bool isOnTime = appointment.Time.GetTimeBeforeStart(DateTime.Now) > 24;
            if (isOnTime)
            {
                var window = new ChangeAppointmentView(selectedAppointment, _doctorRepository, _controller,
                    Appointments, _patient);
                window.Show();
            }
            else
                MessageBox.Show("You are too late", "Error", MessageBoxButton.OK);
        }
        else
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
    }

    public void NewAppointment()
    {
        var window = new MakeAppointmentView(_doctorRepository, _controller, Appointments, _patient);
        window.Show();
    }

    public void CancelAppointmentComm()
    {
        AppointmentViewModel selectedAppointment = SelectedAppointment;
        if (selectedAppointment != null)
        {
            Appointment appointment = _controller.GetAppointmentById(selectedAppointment.Id);
            bool isOnTime = appointment.Time.GetTimeBeforeStart(DateTime.Now) > 24;
            if (isOnTime)
            {
                _controller.CancelAppointment(appointment);
                Appointments.Remove(GetById(selectedAppointment.Id, Appointments));
            }
            else
                MessageBox.Show("You are too late", "Error", MessageBoxButton.OK);
        }
        else
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
    }

    public AppointmentViewModel GetById(int id, ObservableCollection<AppointmentViewModel> Appointments)
    {
        foreach (var appointmentViewModel in Appointments)
        {
            if (appointmentViewModel.Id == id)
            {
                return appointmentViewModel;
            }
        }

        return null;
    }
}