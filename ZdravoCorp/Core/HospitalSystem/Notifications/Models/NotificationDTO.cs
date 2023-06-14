using System;

namespace ZdravoCorp.Core.HospitalSystem.Notifications.Models;

public class NotificationDTO
{
    public int Id { get; }
    public DateTime When { get; set; }
    public string Message { get; set; }
    public string UserEmail { get; set; }
    public Notification.NotificationStatus Status { get; set; }
    public string Source { get; set; }


    public NotificationDTO(int id, DateTime when, string message, string userEmail, Notification.NotificationStatus status, string source)
    {
        Id = id;
        When = when;
        Message = message;
        UserEmail = userEmail;
        Status = status;
        Source = source;
    }
}