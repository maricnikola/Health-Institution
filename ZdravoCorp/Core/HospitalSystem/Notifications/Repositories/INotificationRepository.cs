using System;
using System.Collections.Generic;
using ZdravoCorp.Core.HospitalSystem.Notifications.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalSystem.Notifications.Repositories;

public interface INotificationRepository : IRepository<Notification>
{
    IEnumerable<Notification> GetAll();
    void Insert(Notification notification);
    void Delete(Notification notification);
    Notification GetById(int id);
    void UpdateStatus(int id, Notification.NotificationStatus status);
    void UpdateTime(int id, DateTime when);


}