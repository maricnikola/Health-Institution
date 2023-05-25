using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.Notifications;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Services.NotificationServices;
using ZdravoCorp.View;
using ZdravoCorp.View.PatientView;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class AllNotificationsViewModel : ViewModelBase
{
    private INotificationService _notificationService;
    private ObservableCollection<NotificationViewModel> _notifications;
    private string _userEmail;
    private readonly object _lock;

    public ICommand CreateNotificationCommand { get; set; }
    public AllNotificationsViewModel(INotificationService notificationService, string userEmail)
    {
        _lock = new object();
        _notificationService = notificationService;
        _notifications = new ObservableCollection<NotificationViewModel>();
        BindingOperations.EnableCollectionSynchronization(_notifications, _lock);
        _userEmail = userEmail;
        _notificationService.DataChanged += (s, e) => RefreshNotifications();
        foreach (var notification in _notificationService.GetAllForUser(_userEmail))
        {
            if (notification.Status==Notification.NotificationStatus.Pending)
                _notifications.Add(new NotificationViewModel(notification));
        }
        CreateNotificationCommand = new DelegateCommand(o => CreateNotification());
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

}