using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public ICommand CreateNotificationCommand { get; set; }
    public AllNotificationsViewModel(INotificationService notificationService, string userEmail)
    {
        _notificationService = notificationService;
        _notifications = new ObservableCollection<NotificationViewModel>();
        _userEmail = userEmail;
        foreach (var notification in _notificationService.GetAllForUser(_userEmail))
        {
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

    

    private void CreateNotification()
    {
        var window = new CreateNotificationView()
        {
            DataContext = new CreateNotificationViewModel(_notificationService, Notifications, _userEmail)
        };
        window.Show();
    }

}