using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Models;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Services;
using ZdravoCorp.Core.HospitalSystem.Notifications.Models;
using ZdravoCorp.Core.HospitalSystem.Notifications.Services;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;
using ZdravoCorp.GUI.HospitalSystem.AnnualLeaves.ViewModels;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;

public class ApproveAnnualRequestViewModel : ViewModelBase
{
    public event EventHandler? OnRequestClose;
    private AnnualLeave _request;
    private ObservableCollection<AppointmentForCancelViewModel> _appointments;
    private IScheduleService _scheduleService;
    private IAnnualLeaveService _annualLeaveService;
    private INotificationService _notificationService;

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
    public ApproveAnnualRequestViewModel(IScheduleService scheduleService,IAnnualLeaveService annualLeaveService, AnnualLeave request, INotificationService notificationService)
    {
        _scheduleService = scheduleService;
        _annualLeaveService = annualLeaveService;
        _notificationService = notificationService;
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
            var notification = new NotificationDTO(IDGenerator.GetId(), DateTime.Now,
                $"Otkazan vam je pregled: {appointment.Date.ToString()} !", appointment.Patient,
                Notification.NotificationStatus.Pending, "User made");
            _notificationService.AddNotification(notification);
            JobScheduler.NotificationTaskScheduler(notification);
        }
        _annualLeaveService.Approve(_request.Id);
        OnRequestClose?.Invoke(this, EventArgs.Empty);
        
        
    }
    
}