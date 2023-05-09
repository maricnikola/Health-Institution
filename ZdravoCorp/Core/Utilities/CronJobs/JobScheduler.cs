using System;
using System.Windows.Media;
using Quartz;
using Quartz.Impl;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Repositories.Inventory;

namespace ZdravoCorp.Core.Utilities.CronJobs;

public class JobScheduler
{
    private static ISchedulerFactory _builder;
    private static IScheduler _scheduler;
    public JobScheduler()
    {
        _builder = new StdSchedulerFactory();
        _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        _scheduler.Start();
    }
    // dynamic equipment order task
    public static void DEquipmentTaskScheduler(Order order, InventoryRepository inventoryRepository)
    {
        var job = JobBuilder.Create<DEquipmentExecuteOrder>()
            .WithIdentity(name: "DEquipmentTask" + order.OrderTime , group: "Orders").Build();
        job.JobDataMap["order"] = order;
        job.JobDataMap["invrepo"] = inventoryRepository;
        var trigger = TriggerBuilder.Create().WithIdentity("trigger" + order.ArrivalTime, group: "OrderTriggers")
            .WithCronSchedule("0 " + order.ArrivalTime.Minute + " " + order.ArrivalTime.Hour + " " + " * * ?",
                x => x.InTimeZone(TimeZoneInfo.Local)).ForJob(job).Build();
        _scheduler.ScheduleJob(job, trigger);

    }
}