using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.Notifications.Models;
using ZdravoCorp.Core.HospitalSystem.Notifications.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.Notifications.ViewModels;

public class CreateNotificationViewModel : ViewModelBase
{
    private INotificationService _notificationService;
    private ObservableCollection<NotificationViewModel> _notifications;
    private string _userEmail;
    private int _hours = 0;
    private int _minutes = 0;
    private DateTime _date = DateTime.Now;
    private string _message = "";

    public ICommand CreateNotificationCommand { get; set; }

    public CreateNotificationViewModel(INotificationService notificationService, ObservableCollection<NotificationViewModel> notifications, string userEmail)
    {
        _notificationService = notificationService;
        _notifications = notifications;
        _userEmail = userEmail;
        CreateNotificationCommand = new DelegateCommand(o => CreateNotification());
    }
    public DateTime Date
    {
        get => _date;
        set
        {
            _date = value;
            OnPropertyChanged();
        }
    }
    public int Hours
    {
        get => _hours;
        set
        {
            _hours = value;
            OnPropertyChanged();
        }
    }
    public int Minutes
    {
        get => _minutes;
        set
        {
            _minutes = value;
            OnPropertyChanged();
        }
    }
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    private void CreateNotification()
    {
        var when = new DateTime(Date.Year, Date.Month, Date.Day, Hours, Minutes, 0);
        if (CheckInput(when)) return;
        var notificationDto = new NotificationDTO(IDGenerator.GetId(), when, Message, _userEmail,Notification.NotificationStatus.Pending, "User made");
        _notificationService.AddNotification(notificationDto);
        _notifications.Add(new NotificationViewModel(new Notification(notificationDto)));
        JobScheduler.NotificationTaskScheduler(notificationDto);
    }

    private bool CheckInput(DateTime when)
    {

        if (Hours < 0 || Hours > 24 || Minutes < 0 || Minutes > 60)
        {
            MessageBox.Show("Bad Time", "Error", MessageBoxButton.OK);
            return true;
        }

        if (Message == "")
        {
            MessageBox.Show("No message written", "Error", MessageBoxButton.OK);
            return true;
        }

        when = new DateTime(Date.Year, Date.Month, Date.Day, Hours, Minutes, 0);
        if (when >= DateTime.Now) return false;
        MessageBox.Show("That the past!", "Error", MessageBoxButton.OK);
        return true;

    }
}