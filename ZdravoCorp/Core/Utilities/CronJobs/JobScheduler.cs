using System;
using System.Collections.Specialized;
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
    private static InventoryRepository _inventoryRepository;
    private static OrderRepository _orderRepository;
    private static TransferRepository _transferRepository;

    public JobScheduler(InventoryRepository inventoryRepository, TransferRepository transferRepository, OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
        _inventoryRepository = inventoryRepository;
        _transferRepository = transferRepository;
        _builder = new StdSchedulerFactory();
        _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        _scheduler.Start();
        LoadScheduledTasks();
    }

    private void LoadScheduledTasks()
    {
        foreach (var order in _orderRepository.GetOrders())
        {
            if (order.Status == Order.OrderStatus.Pending)
            {
                DEquipmentTaskScheduler(order);
            }
        }

        foreach (var transfer in _transferRepository.GetAll())
        {
            TransferRequestTaskScheduler(transfer);
        }
    }

    // dynamic equipment order task
    public static void DEquipmentTaskScheduler(Order order)
    {
        var job = JobBuilder.Create<DEquipmentExecuteOrder>()
            .WithIdentity(name: "DEquipmentTask" + order.OrderTime, group: "Orders").Build();
        job.JobDataMap["order"] = order;
        job.JobDataMap["invrepo"] = _inventoryRepository;
        job.JobDataMap["ordrepo"] = _orderRepository;
         var trigger = TriggerBuilder.Create().WithIdentity("trigger" + order.ArrivalTime, group: "OrderTriggers")
            .WithCronSchedule("0 " + order.ArrivalTime.Minute + " " + order.ArrivalTime.Hour + " " +  order.ArrivalTime.Day + " " + order.ArrivalTime.Month + " ? *",
                x => x.InTimeZone(TimeZoneInfo.Local)).ForJob(job).Build();
        _scheduler.ScheduleJob(job, trigger);
    }

    // equipment transfer task

    public static void TransferRequestTaskScheduler(Transfer transfer)
    {
        var job = JobBuilder.Create<TransferRequestTask>()
            .WithIdentity(name: "TrasferRequest" + transfer.Id, group: "Transfers").Build();
        job.JobDataMap["transfer"] = transfer;
        job.JobDataMap["invrepo"] = _inventoryRepository;
        job.JobDataMap["transrepo"] = _transferRepository;

        var trigger = TriggerBuilder.Create()
            .WithIdentity("transfer trigger" + transfer.When, group: "TransferTriggers").WithCronSchedule(
                "0 " + transfer.When.Minute + " " + transfer.When.Hour + " " + transfer.When.Day + " " +
                transfer.When.Month + " ? *", x => x.InTimeZone(TimeZoneInfo.Local)).ForJob(job).Build();
        _scheduler.ScheduleJob(job, trigger);
    }
}