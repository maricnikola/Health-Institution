using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Models.Notifications;

public class Notification
{
    public int Id { get; }
    public DateTime When { get; set; }
    public string Message { get; set; }
    public string UserEmail { get; set; }
    public NotificationStatus Status { get; set; }

    public enum NotificationStatus
    {
        Pending,
        Completed
    }

    //probably status

    [JsonConstructor]
    public Notification(int id, DateTime when, string message, string userEmail,NotificationStatus status)
    {
        Id = id;
        When = when;
        Message = message;
        UserEmail = userEmail;
        Status = status;
    }

    public Notification(NotificationDTO notificationDto)
    {
        Id=notificationDto.Id;
        When = notificationDto.When;
        Message = notificationDto.Message;
        UserEmail = notificationDto.UserEmail;
        Status = notificationDto.Status;
    }
}