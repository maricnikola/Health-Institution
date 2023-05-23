using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Notifications;
using ZdravoCorp.Core.Services.NotificationServices;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class AllNotificationsViewModel : ViewModelBase
{
    private INotificationService _notificationService;
    private ObservableCollection<NotificationViewModel> _notifications;
    public AllNotificationsViewModel(INotificationService notificationService)
    {
        _notificationService = notificationService;
        _notifications = new ObservableCollection<NotificationViewModel>();
        foreach (var notification in _notificationService.GetAll())
        {
            _notifications.Add(new NotificationViewModel(notification));
        }
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

}