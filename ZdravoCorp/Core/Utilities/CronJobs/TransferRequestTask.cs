using System;
using System.Threading.Tasks;
using Quartz;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.TransfersRepo;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.TransferServices;

namespace ZdravoCorp.Core.Utilities.CronJobs;

public class TransferRequestTask : IJob
{
    private IInventoryService _inventoryService;
    private Transfer _transfer;
    private ITransferService _transferService;

    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        _transfer = (Transfer)dataMap["transfer"];
        _inventoryService = (IInventoryService)dataMap["invser"];
        _transferService = (ITransferService)dataMap["transser"];
        _inventoryService.UpdateInventoryItem(_transfer);


        _transferService.Delete(_transfer.Id);
        _transferRepository.OnRequestUpdate(this, new EventArgs());
        _inventoryRepository.OnRequestUpdate(this, new EventArgs());

        return Task.CompletedTask;
    }
}