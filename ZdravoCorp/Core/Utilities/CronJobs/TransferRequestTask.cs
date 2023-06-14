using System;
using System.Threading.Tasks;
using Quartz;
using ZdravoCorp.Core.HospitalAssets.Equipment.Models;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;

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