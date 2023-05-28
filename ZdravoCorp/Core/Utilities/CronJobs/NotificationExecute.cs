using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Quartz;
using ZdravoCorp.Core.Models.Notifications;
using ZdravoCorp.Core.Services.NotificationServices;

namespace ZdravoCorp.Core.Utilities.CronJobs;

public class NotificationExecute : IJob
{
    private INotificationService _notificationService;
    private NotificationDTO _notification;
    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        _notification = (NotificationDTO)dataMap["notification"];
        _notificationService = (INotificationService)dataMap["notserv"];
        MessageBox.Show(_notification.Message, "NOTIFICATION", MessageBoxButton.OK);
        _notificationService.UpdateStatus(_notification.Id,Notification.NotificationStatus.Completed);

        return Task.CompletedTask;
    }
}