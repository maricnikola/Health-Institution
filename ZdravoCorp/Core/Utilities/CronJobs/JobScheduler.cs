﻿using System;
using Quartz;
using Quartz.Impl;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.OrderRepo;
using ZdravoCorp.Core.Repositories.TransfersRepo;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.OrderServices;
using ZdravoCorp.Core.Services.TransferServices;

namespace ZdravoCorp.Core.Utilities.CronJobs;

public class JobScheduler
{
    private static ISchedulerFactory _builder;
    private static IScheduler _scheduler;
    private static IInventoryService _inventoryService;
    private static IOrderService _orderService;
    private static ITransferService _transferService;

    public JobScheduler()
    {

        _builder = new StdSchedulerFactory();
        _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        _scheduler.Start();
        LoadScheduledTasks();
    }

    private void LoadScheduledTasks()
    {
        foreach (var order in _orderService.GetAll())
            if (order.Status == Order.OrderStatus.Pending)
                DEquipmentTaskScheduler(new OrderDTO(order.Id, order.Items, order.OrderTime, order.ArrivalTime, order.Status));

        foreach (var transfer in _transferService.GetAll()) TransferRequestTaskScheduler(new TransferDTO(transfer.Id, transfer.From, transfer.To, transfer.When, transfer.Quantity, transfer.InventoryId, transfer.InventoryItemName));
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
}