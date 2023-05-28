using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Notifications;
using ZdravoCorp.Core.Models.Orders;

namespace ZdravoCorp.Core.Repositories.NotificationRepo;

public interface INotificationRepository : IRepository<Notification>
{
    IEnumerable<Notification> GetAll();
    void Insert(Notification notification);
    void Delete(Notification notification);
    Notification GetById(int id);
    void UpdateStatus(int id, Notification.NotificationStatus status);
    void UpdateTime(int id, DateTime when);


}