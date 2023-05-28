using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Notifications;

namespace ZdravoCorp.Core.ViewModels;

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