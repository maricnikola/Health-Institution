using System;
using System.Threading.Tasks;
using Quartz;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Repositories.Inventory;

namespace ZdravoCorp.Core.Utilities.CronJobs;

public class DEquipmentExecuteOrder : IJob
{
    private Order _order;
    private InventoryRepository _inventoryRepository;

    public Task Execute(IJobExecutionContext context)
    {
        JobDataMap dataMap = context.JobDetail.JobDataMap;
        _order = (Order)dataMap["order"];
        _inventoryRepository = (InventoryRepository)dataMap["invrepo"];
        foreach (var item in _order.Items)
        {
            _inventoryRepository.AddItem(new InventoryItem(IDGenerator.GetId(), item.Value, 999, item.Key));
        }

        _inventoryRepository.LoadRoomsAndEquipment();
        _inventoryRepository.OnRequestUpdate(this, new EventArgs());
        

        return Task.CompletedTask;
    }
}