using ZdravoCorp.Core.HospitalAssets.Equipment.Models;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalAssets.Equipment.ViewModels;

public class OrderViewModel : ViewModelBase
{
    private readonly Order _order;


    public OrderViewModel(Order order, string items)
    {
        _order = order;
        Items = items;
    }

    public int Id => _order.Id;

    public string Items { get; set; }
    public string OrderTime => _order.OrderTime.ToString();
    public string ArrivalTime => _order.ArrivalTime.ToString();
    public string Status => _order.Status.ToString();
}