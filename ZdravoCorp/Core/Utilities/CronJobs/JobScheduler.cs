using System;
using System.Collections.Specialized;
using Autofac;
using Quartz;
using Quartz.Impl;
using ZdravoCorp.Core.HospitalAssets.Equipment.Models;
using ZdravoCorp.Core.HospitalAssets.Rooms.Models;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;
using ZdravoCorp.Core.HospitalSystem.Notifications.Models;
using ZdravoCorp.Core.HospitalSystem.Notifications.Services;

namespace ZdravoCorp.Core.Utilities.CronJobs;

public class JobScheduler
{
    private static ISchedulerFactory _builder;
    private static IScheduler _scheduler;
    private static IInventoryService _inventoryService;
    private static IOrderService _orderService;
    private static ITransferService _transferService;
    private static IRenovationService _renovationService;
    private static IManageRenovationService _manageRenovationService;
    private static INotificationService _notificationService;

    public JobScheduler()
    {
        _inventoryService = Injector.Container.Resolve<IInventoryService>();
        _orderService = Injector.Container.Resolve<IOrderService>();
        _transferService = Injector.Container.Resolve<ITransferService>();
        _renovationService = Injector.Container.Resolve<IRenovationService>();
        _manageRenovationService = Injector.Container.Resolve<IManageRenovationService>();
        _notificationService = Injector.Container.Resolve<INotificationService>();
        var properties = new NameValueCollection { {"quartz.threadPool.threadCount", "1"} };
        _builder = new StdSchedulerFactory(properties);
        _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        _scheduler.Start();
        LoadScheduledTasks();
    }

    public static void RefreshScheduledTasks(string username)
    {
        _scheduler.Clear();
        LoadScheduledTasks();
        LoadUsersNotifications(username);
    }

    private static void LoadScheduledTasks()
    {
        foreach (var order in _orderService.GetAll())
            if (order.Status == Order.OrderStatus.Pending)
                DEquipmentTaskScheduler(new OrderDTO(order.Id, order.Items, order.OrderTime, order.ArrivalTime, order.Status));
        foreach (var transfer in _transferService.GetAll())
            if (transfer.Status == Transfer.TransferStatus.Pending)
                TransferRequestTaskScheduler(new TransferDTO(transfer.Id, transfer.From, transfer.To, transfer.When, transfer.Quantity, transfer.InventoryId, transfer.InventoryItemName, transfer.Status));
        foreach (var renovation in _renovationService.GetAll())
        {
            if (renovation.Status == Renovation.RenovationStatus.Pending || renovation.Status == Renovation.RenovationStatus.InProgress)
                RenovationTaskScheduler(new RenovationDTO(renovation.Id, renovation.Room, renovation.Slot, renovation.Status, renovation.Split, renovation.Join));
        }
    }

    public static void LoadUsersNotifications(string userEmail)
    {
        foreach (var notification in _notificationService.GetAllForUser(userEmail))
            if (notification.Status == Notification.NotificationStatus.Pending)
                NotificationTaskScheduler(new NotificationDTO(notification.Id, notification.When, notification.Message, notification.UserEmail, notification.Status,notification.Source));
    }

    // dynamic equipment order task
    public static void DEquipmentTaskScheduler(OrderDTO order)
    {
        var job = JobBuilder.Create<DEquipmentExecuteOrder>()
            .WithIdentity("DEquipmentTask" + order.OrderTime, "Orders").Build();
        job.JobDataMap["order"] = order;
        job.JobDataMap["invser"] = _inventoryService;
        job.JobDataMap["ordser"] = _orderService;
        ITrigger trigger;
        if (order.ArrivalTime < DateTime.Now)
            trigger = TriggerBuilder.Create().WithIdentity("trigger" + order.ArrivalTime, "OrderTriggers")
                .StartNow().ForJob(job).Build();
        else
            trigger = TriggerBuilder.Create().WithIdentity("trigger" + order.ArrivalTime, "OrderTriggers")
                .WithCronSchedule(
                    "0 " + order.ArrivalTime.Minute + " " + order.ArrivalTime.Hour + " " + order.ArrivalTime.Day + " " +
                    order.ArrivalTime.Month + " ? " + order.ArrivalTime.Year,
                    x => x.InTimeZone(TimeZoneInfo.Local).WithMisfireHandlingInstructionFireAndProceed()).ForJob(job)
                .Build();

        _scheduler.ScheduleJob(job, trigger);
    }

    // equipment transfer task

    public static void TransferRequestTaskScheduler(TransferDTO transfer)
    {
        var job = JobBuilder.Create<TransferRequestTask>()
            .WithIdentity("TrasferRequest" + transfer.Id, "Transfers").Build();
        job.JobDataMap["transfer"] = transfer;
        job.JobDataMap["invser"] = _inventoryService;
        job.JobDataMap["transser"] = _transferService;
        ITrigger trigger;
        if (transfer.When < DateTime.Now)
            trigger = TriggerBuilder.Create()
                .WithIdentity("transfer trigger" + transfer.When, "TransferTriggers").StartNow().ForJob(job)
                .Build();
        else
            trigger = TriggerBuilder.Create()
                .WithIdentity("transfer trigger" + transfer.When, "TransferTriggers").WithCronSchedule(
                    "0 " + transfer.When.Minute + " " + transfer.When.Hour + " " + transfer.When.Day + " " +
                    transfer.When.Month + " ? " + transfer.When.Year,
                    x => x.InTimeZone(TimeZoneInfo.Local).WithMisfireHandlingInstructionFireAndProceed()).ForJob(job)
                .Build();

        _scheduler.ScheduleJob(job, trigger);
    }

    public static void RenovationTaskScheduler(RenovationDTO renovation)
    {
        if (renovation.Status != Renovation.RenovationStatus.InProgress && renovation.Slot.End > DateTime.Now)
        {
            var startJob = JobBuilder.Create<StartRenovationTask>()
                .WithIdentity("RenovationStart" + renovation.Id, "Renovations").Build();
            ITrigger startTrigger;
            startJob.JobDataMap["renovation"] = renovation;
            startJob.JobDataMap["renser"] = _renovationService;
            startJob.JobDataMap["renman"] = _manageRenovationService;
            if (renovation.Slot.Start < DateTime.Now)
                startTrigger = TriggerBuilder.Create()
                    .WithIdentity("RenovationStartTrigger" + renovation.Slot.Start, "RenovationTriggers").StartNow()
                    .ForJob(startJob)
                    .Build();
            else
                startTrigger = TriggerBuilder.Create()
                    .WithIdentity("RenovationStartTrigger" + renovation.Slot.Start, "RenovationTriggers")
                    .WithCronSchedule(
                        "0 " + renovation.Slot.Start.Minute + " " + renovation.Slot.Start.Hour + " " +
                        renovation.Slot.Start.Day + " " +
                        renovation.Slot.Start.Month + " ? " + renovation.Slot.Start.Year,
                        x => x.InTimeZone(TimeZoneInfo.Local).WithMisfireHandlingInstructionFireAndProceed())
                    .ForJob(startJob)
                    .Build();

            _scheduler.ScheduleJob(startJob, startTrigger);
        }

        var endJob = JobBuilder.Create<EndRenovationTask>()
            .WithIdentity("RenovationEnd" + renovation.Id, "Renovations").Build();
        ITrigger endTrigger;
        endJob.JobDataMap["renovation"] = renovation;
        endJob.JobDataMap["renser"] = _renovationService;
        endJob.JobDataMap["renman"] = _manageRenovationService;
        
        if (renovation.Slot.End < DateTime.Now)
            endTrigger = TriggerBuilder.Create()
                .WithIdentity("RenovationEndTrigger" + renovation.Slot.End, "RenovationTriggers").StartNow().ForJob(endJob)
                .Build();
        else
            endTrigger = TriggerBuilder.Create()
                .WithIdentity("RenovationEndTrigger" + renovation.Slot.End, "RenovationTriggers").WithCronSchedule(
                    "0 " + renovation.Slot.End.Minute + " " + renovation.Slot.End.Hour + " " + renovation.Slot.End.Day + " " +
                    renovation.Slot.End.Month + " ? " + renovation.Slot.End.Year,
                    x => x.InTimeZone(TimeZoneInfo.Local).WithMisfireHandlingInstructionFireAndProceed()).ForJob(endJob)
                .Build();

        _scheduler.ScheduleJob(endJob, endTrigger);
        
    }

    public static void NotificationTaskScheduler(NotificationDTO notification)
    {
        var job = JobBuilder.Create<NotificationExecute>()
            .WithIdentity("NotificationTasak" + notification.Id, "Notifications").Build();
        job.JobDataMap["notification"] = notification;
        job.JobDataMap["notserv"] = _notificationService;
        ITrigger trigger;
        if (notification.When<DateTime.Now)
            trigger = TriggerBuilder.Create()
                .WithIdentity("notification trigger" + notification.When, "NotificationTriggers").StartNow().ForJob(job)
                .Build();
        else
            trigger = TriggerBuilder.Create()
                .WithIdentity("notification trigger" + notification.When, "NotificationTriggers").WithCronSchedule(
                    "0 " + notification.When.Minute + " " + notification.When.Hour + " " + notification.When.Day + " " +
                    notification.When.Month + " ? " + notification.When.Year,
                    x => x.InTimeZone(TimeZoneInfo.Local).WithMisfireHandlingInstructionFireAndProceed()).ForJob(job)
                .Build();

        _scheduler.ScheduleJob(job, trigger);
    }
}