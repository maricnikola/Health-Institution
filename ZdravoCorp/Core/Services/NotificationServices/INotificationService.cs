using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Notifications;
using ZdravoCorp.Core.Models.Presriptions;

namespace ZdravoCorp.Core.Services.NotificationServices;

public interface INotificationService
{
    public List<Notification>? GetAll();

    public event EventHandler DataChanged;
    public Notification? GetById(int id);
    public void AddNotification(NotificationDTO notificationDto);
    public void Delete(int id);
    public List<Notification> GetAllForUser(string userEmail);
    public void UpdateStatus(int id, Notification.NotificationStatus status);
    public void CreateNotificationsFromPrescriptions(List<Prescription> prescriptions, TimeSpan timeForNotification, string email);
    public void UpdateNotificationsForMedicine(string email, TimeSpan oldTimeSpan, TimeSpan newTimeSpan);

}