using System;
using System.Windows.Media;
using Quartz;
using Quartz.Impl;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.Core.Repositories.Transfers;

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
    public static void DEquipmentTaskScheduler(Order order, InventoryRepository inventoryRepository, OrderRepository orderRepository)
    {
        var job = JobBuilder.Create<DEquipmentExecuteOrder>()
            .WithIdentity(name: "DEquipmentTask" + order.OrderTime, group: "Orders").Build();
        job.JobDataMap["order"] = order;
        job.JobDataMap["invrepo"] = inventoryRepository;
        job.JobDataMap["ordrepo"] = orderRepository;
         var trigger = TriggerBuilder.Create().WithIdentity("trigger" + order.ArrivalTime, group: "OrderTriggers")
            .WithCronSchedule("0 " + order.ArrivalTime.Minute + " " + order.ArrivalTime.Hour + " " +  order.ArrivalTime.Day + " " + order.ArrivalTime.Month + " ? *",
                x => x.InTimeZone(TimeZoneInfo.Local)).ForJob(job).Build();
        _scheduler.ScheduleJob(job, trigger);
    }

    // equipment transfer task

    public static void TransferRequestTaskScheduler(Transfer transfer, InventoryRepository inventoryRepository, TransferRepository transferRepository)
    {
        var job = JobBuilder.Create<TransferRequestTask>()
            .WithIdentity(name: "TrasferRequest" + transfer.Id, group: "Transfers").Build();
        job.JobDataMap["transfer"] = transfer;
        job.JobDataMap["invrepo"] = inventoryRepository;
        job.JobDataMap["transrepo"] = transferRepository;

        var trigger = TriggerBuilder.Create()
            .WithIdentity("transfer trigger" + transfer.When, group: "TransferTriggers").WithCronSchedule(
                "0 " + transfer.When.Minute + " " + transfer.When.Hour + " " + transfer.When.Day + " " +
                transfer.When.Month + " ? *", x => x.InTimeZone(TimeZoneInfo.Local)).ForJob(job).Build();
        _scheduler.ScheduleJob(job, trigger);
    }
}