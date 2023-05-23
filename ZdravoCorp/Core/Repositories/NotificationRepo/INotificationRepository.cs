using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Notifications;

namespace ZdravoCorp.Core.Repositories.NotificationRepo;

public interface INotificationRepository : IRepository<Notification>
{
    IEnumerable<Notification> GetAll();
    void Insert(Notification notification);
    void Delete(Notification notification);
    Notification GetById(int id);

}