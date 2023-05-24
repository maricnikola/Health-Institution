﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Accessibility;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Notifications;
using ZdravoCorp.Core.Services.NotificationServices;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

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
        if (Hours < 0 || Hours > 24 || Minutes < 0 || Minutes > 60)
        {
            MessageBox.Show("Bad Time", "Error", MessageBoxButton.OK);
            return;
        }
        if (Message == "")
        {
            MessageBox.Show("No message written", "Error", MessageBoxButton.OK);
            return;
        }
        var when = new DateTime(Date.Year, Date.Month, Date.Day, Hours, Minutes, 0);
        if (when < DateTime.Now)
        {
            MessageBox.Show("That the past!", "Error", MessageBoxButton.OK);
            return;
        }
        var notificationDto = new NotificationDTO(IDGenerator.GetId(), when, Message, _userEmail,Notification.NotificationStatus.Pending);
        _notificationService.AddNotification(notificationDto);
        _notifications.Add(new NotificationViewModel(new Notification(notificationDto)));
        JobScheduler.NotificationTaskScheduler(notificationDto);
    }

}