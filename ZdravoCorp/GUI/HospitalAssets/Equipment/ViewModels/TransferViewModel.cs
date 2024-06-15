using ZdravoCorp.Core.HospitalAssets.Equipment.Models;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalAssets.Equipment.ViewModels;

public class TransferViewModel : ViewModelBase
{
    private readonly Transfer _transfer;


    public TransferViewModel(Transfer transfer)
    {
        _transfer = transfer;
    }

    public string? Item => _transfer.InventoryItemName;
    public string From => _transfer.From.Id.ToString();
    public string To => _transfer.To.Id.ToString();
    public string When => _transfer.When.ToString();
    public int Quantity => _transfer.Quantity;
    public string Status => _transfer.Status.ToString();
}