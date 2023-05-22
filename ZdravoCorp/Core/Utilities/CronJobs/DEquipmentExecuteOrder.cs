using System;
using System.Threading.Tasks;
using Quartz;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.OrderRepo;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.OrderServices;

namespace ZdravoCorp.Core.Utilities.CronJobs;

public class DEquipmentExecuteOrder : IJob
{
    private IInventoryService _inventoryService;
    private OrderDTO _order;
    private IOrderService _orderService;

    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        _order = (OrderDTO)dataMap["order"];
        _inventoryService = (IInventoryService)dataMap["invser"];
        _orderService = (IOrderService)dataMap["ordser"];
        foreach (var item in _order.Items)
            _inventoryService.AddFromOrder(new InventoryItem(IDGenerator.GetId(), item.Value, 999, item.Key));
        _orderService.UpdateStatus(_order.Id, Order.OrderStatus.Completed);

        return Task.CompletedTask;
    }
}