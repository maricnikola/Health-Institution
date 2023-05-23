using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Notifications;

namespace ZdravoCorp.Core.Services.NotificationServices;

public interface INotificationService
{
    public List<Notification>? GetAll();
    public Notification? GetById(int id);
    public void AddNotification(NotificationDTO notificationDto);
    public void Delete(int id);
}