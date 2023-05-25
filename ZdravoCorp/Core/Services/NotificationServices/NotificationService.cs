using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Notifications;
using ZdravoCorp.Core.Repositories.NotificationRepo;

namespace ZdravoCorp.Core.Services.NotificationServices;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }
    public List<Notification>? GetAll()
    {
        return _notificationRepository.GetAll() as List<Notification>;
    }

    public event EventHandler? DataChanged;

    public Notification? GetById(int id)
    {
        return _notificationRepository.GetById(id);
    }

    public void AddNotification(NotificationDTO notificationDto)
    {
        _notificationRepository.Insert(new Notification(notificationDto));
        DataChanged?.Invoke(this, new EventArgs());
    }

    public void Delete(int id)
    {
        _notificationRepository.Delete(GetById(id));
        DataChanged?.Invoke(this, new EventArgs());
    }

    public List<Notification> GetAllForUser(string userEmail)
    {
        return GetAll().Where(notification => notification.UserEmail == userEmail).ToList();
    }

    public void UpdateStatus(int id, Notification.NotificationStatus status)
    {
        _notificationRepository.UpdateStatus(id, status);
        DataChanged?.Invoke(this, new EventArgs());
    }
}