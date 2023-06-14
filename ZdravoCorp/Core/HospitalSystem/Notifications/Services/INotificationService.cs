using System;
using System.Collections.Generic;
using ZdravoCorp.Core.HospitalSystem.Notifications.Models;
using ZdravoCorp.Core.PatientFiles.Presriptions.Models;

namespace ZdravoCorp.Core.HospitalSystem.Notifications.Services;

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