using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.Notifications;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Services.NotificationServices;
using ZdravoCorp.Core.Services.PatientServices;
using ZdravoCorp.View;
using ZdravoCorp.View.PatientView;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class AllNotificationsViewModel : ViewModelBase
{
    private INotificationService _notificationService;
    private IPatientService _patientService;
    private ObservableCollection<NotificationViewModel> _notifications;
    private Patient _patient;

    private string _userEmail;
    private readonly object _lock;

    public ICommand CreateNotificationCommand { get; set; }
    public ICommand ChangeNotificationTimeCommand { get; set; }
    public AllNotificationsViewModel(INotificationService notificationService, IPatientService patientService ,string userEmail)
    {
        _lock = new object();
        _notificationService = notificationService;
        _patientService = patientService;
        _notifications = new ObservableCollection<NotificationViewModel>();
        BindingOperations.EnableCollectionSynchronization(_notifications, _lock);
        _patient = _patientService.GetByEmail(userEmail);
        _hours = _patient.NotificationTime.Hours;
        _minutes = _patient.NotificationTime.Minutes;

        _userEmail = userEmail;
        _notificationService.DataChanged += (s, e) => RefreshNotifications();
        foreach (var notification in _notificationService.GetAllForUser(_userEmail))
        {
            if (notification.Status==Notification.NotificationStatus.Pending)
                _notifications.Add(new NotificationViewModel(notification));
        }
        CreateNotificationCommand = new DelegateCommand(o => CreateNotification());
        ChangeNotificationTimeCommand = new DelegateCommand(o => ChangeNotificationTime());
    }

    public ObservableCollection<NotificationViewModel> Notifications
    {
        get => _notifications;
        set
        {
            _notifications = value;
            OnPropertyChanged();
        }
    }

    private int _hours;
    public int Hours
    {
        get => _hours;
        set
        {
            _hours = value;
            OnPropertyChanged();
        }
    }
    private int _minutes;
    public int Minutes
    {
        get => _minutes;
        set
        {
            _minutes = value;
            OnPropertyChanged();
        }
    }

    private void RefreshNotifications()
    {
        lock (_lock)
        {
            var updatedNotifications = new ObservableCollection<NotificationViewModel>();
            foreach (var notification in _notificationService.GetAllForUser(_userEmail))
                if(notification.Status==Notification.NotificationStatus.Pending)
                    updatedNotifications.Add(new NotificationViewModel(notification));
            Notifications = updatedNotifications;
        }
    }

    private void CreateNotification()
    {
        var window = new CreateNotificationView()
        {
            DataContext = new CreateNotificationViewModel(_notificationService, Notifications, _userEmail)
        };
        window.Show();
    }

    private void ChangeNotificationTime()
    {
        if (CheckTimeInput()) return;
        var newTimeSpan = new TimeSpan(Hours, Minutes, 0);
        var oldTimeSpan = new TimeSpan(_patient.NotificationTime.Hours, _patient.NotificationTime.Minutes, 0);
        _patientService.ChangeTimeForNotification(_patient.Email,newTimeSpan);
        _notificationService.UpdateNotificationsForMedicine(_patient.Email,oldTimeSpan,newTimeSpan);
        MessageBox.Show("Successfully changed", "Success", MessageBoxButton.OK);

    }

    private bool CheckTimeInput()
    {
        if (Hours < 0 || Hours > 24 || Minutes < 0 || Minutes > 59)
        {
            MessageBox.Show("Bad Time", "Error", MessageBoxButton.OK);
            return true;
        }
        if (Hours != _patient.NotificationTime.Hours || Minutes != _patient.NotificationTime.Minutes) return false;
        MessageBox.Show("Nothing is changed", "Error", MessageBoxButton.OK);
        return true;

    }


}