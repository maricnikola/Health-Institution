using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.HospitalSystem.Notifications.Models;

public class Notification
{
    public int Id { get; }
    public DateTime When { get; set; }
    public string Message { get; set; }
    public string UserEmail { get; set; }
    public NotificationStatus Status { get; set; }
    public string Source { get; set; }

    public enum NotificationStatus
    {
        Pending,
        Completed
    }

    //probably status

    [JsonConstructor]
    public Notification(int id, DateTime when, string message, string userEmail,NotificationStatus status, string source)
    {
        Id = id;
        When = when;
        Message = message;
        UserEmail = userEmail;
        Status = status;
        Source = source;
    }

    public Notification(NotificationDTO notificationDto)
    {
        Id=notificationDto.Id;
        When = notificationDto.When;
        Message = notificationDto.Message;
        UserEmail = notificationDto.UserEmail;
        Status = notificationDto.Status;
        Source = notificationDto.Source;
    }
}