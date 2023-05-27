using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.Notifications;
using ZdravoCorp.Core.Models.Presriptions;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.NotificationRepo;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Utilities.CronJobs;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Services.NotificationServices;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IScheduleService _scheduleService;

    public NotificationService(INotificationRepository notificationRepository,IScheduleService scheduleService)
    {
        _notificationRepository = notificationRepository;
        _scheduleService = scheduleService;
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

    public void CreateNotificationsFromPrescriptions(List<Prescription> prescriptions, TimeSpan notificationTime, string email)
    {
        foreach (var prescription in prescriptions)
        {
            var today = DateTime.Today;
            while (today <= prescription.ExpirationDate)
            {
                CreateNotificationForOneDayByPrescription(prescription, today, notificationTime, email);
                today = today.AddDays(1);
            }
        }
    }

    private void CreateNotificationForOneDayByPrescription(Prescription prescription, DateTime today, TimeSpan notificationTime, string email)
    {
        foreach (var hour in prescription.HourlyRates)
        {
            var when = new DateTime(today.Year, today.Month, today.Day, hour, 0, 0);
            var whenNotif = when - notificationTime;
            if (whenNotif < DateTime.Now) //inspect this better
                continue;
            var message = $"You have to take {prescription.Medicament} medicine at {when:hh:mm:ss tt}";
            var notification = new NotificationDTO(IDGenerator.GetId(), whenNotif, message,
                email, Notification.NotificationStatus.Pending, "For Medication");
            AddNotification(notification);
            JobScheduler.NotificationTaskScheduler(notification);
        }
    }
    public void UpdateNotificationsForMedicine(string email, TimeSpan oldTimeSpan, TimeSpan newTimeSpan)
    {
        var notifications = GetAllForUser(email);
        foreach (var notification in notifications)
        {
            if (!notification.Source.Equals("For Medication")) continue;
            var when = notification.When + oldTimeSpan - newTimeSpan;
            _notificationRepository.UpdateTime(notification.Id, when);
        }
        JobScheduler.RefreshScheduledTasks(email);
        DataChanged.Invoke(this, new EventArgs());
    }
}