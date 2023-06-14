using ZdravoCorp.Core.HospitalAssets.Equipment.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalAssets.Equipment.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    void UpdateStatus(int id, Order.OrderStatus status);
}