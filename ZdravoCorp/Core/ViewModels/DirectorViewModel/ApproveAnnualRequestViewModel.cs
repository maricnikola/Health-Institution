using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.AnnualLeaves;
using ZdravoCorp.Core.Services.AnnualLeaveServices;
using ZdravoCorp.Core.Services.ScheduleServices;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class ApproveAnnualRequestViewModel : ViewModelBase
{
    public event EventHandler? OnRequestClose;
    private AnnualLeave _request;
    private ObservableCollection<AppointmentForCancelViewModel> _appointments;
    private IScheduleService _scheduleService;
    private IAnnualLeaveService _annualLeaveService;

    public IEnumerable<AppointmentForCancelViewModel> Appointments
    {
        get => _appointments;
        set
        {
            _appointments = new ObservableCollection<AppointmentForCancelViewModel>(value);
            OnPropertyChanged();
        }
    }
    private void Exit()
    {
        OnRequestClose?.Invoke(this, new EventArgs());
    }
    
    public ICommand Cancel { get; set; }
    public ICommand Confirm { get; set; }
    public ApproveAnnualRequestViewModel(IScheduleService scheduleService,IAnnualLeaveService annualLeaveService, AnnualLeave request)
    {
        _scheduleService = scheduleService;
        _annualLeaveService = annualLeaveService;
        _request = request;
        _appointments = new ObservableCollection<AppointmentForCancelViewModel>();
        Cancel = new DelegateCommand(o => Exit());
        Confirm = new DelegateCommand(o => ConfirmRequest());
        foreach (var appointment in _scheduleService.GetDoctorAppointmentsInTimeSlot(request.DoctorMail, request.Time))
        {
            _appointments.Add(new AppointmentForCancelViewModel(appointment.Id, appointment.PatientEmail, appointment.Time.Start.ToString()));
        }
    }

    private void ConfirmRequest()
    {
        foreach (var appointment in _appointments)
        {
            _scheduleService.CancelAppointment(appointment.Id);
        }
        _annualLeaveService.Approve(_request.Id);
        OnRequestClose?.Invoke(this, EventArgs.Empty);
        
        
    }
    
}