using ZdravoCorp.Core.Models.Orders;

namespace ZdravoCorp.Core.Repositories.OrderRepo;

public interface IOrderRepository : IRepository<Order>
{
    void UpdateStatus(int id, Order.OrderStatus status);
}