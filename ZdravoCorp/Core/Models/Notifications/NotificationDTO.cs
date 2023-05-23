using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Models.Notifications;

public class NotificationDTO
{
    public int Id { get; }
    public DateTime When { get; set; }
    public string Message { get; set; }
    public string UserEmail { get; set; }

    public NotificationDTO(int id, DateTime when, string message, string userEmail)
    {
        Id = id;
        When = when;
        Message = message;
        UserEmail = userEmail;
    }
}