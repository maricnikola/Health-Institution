using System;
using ZdravoCorp.Core.HospitalSystem.Notifications.Models;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.Notifications.ViewModels;

public class NotificationViewModel : ViewModelBase
{
    private readonly Notification _notification;

    public NotificationViewModel(Notification notification)
    {
        _notification = notification;
    }

    public DateTime When =>_notification.When;
    public string Message => _notification.Message;
    public string Source => _notification.Source;
}