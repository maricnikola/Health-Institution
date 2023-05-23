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
    private InventoryService _inventoryService;
    private TransferDTO _transfer;
    private TransferService _transferService;

    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        _transfer = (TransferDTO)dataMap["transfer"];
        _inventoryService = (InventoryService)dataMap["invser"];
        _transferService = (TransferService)dataMap["transser"];
        if (_inventoryService.UpdateInventoryItem(new Transfer(_transfer)))
        {
            _transferService.UpdateStatus(_transfer.Id, Transfer.TransferStatus.Completed);
        }

        else
        {
            _transferService.UpdateStatus(_transfer.Id, Transfer.TransferStatus.Failed);
        }




        return Task.CompletedTask;
    }
}