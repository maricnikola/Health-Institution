using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DEquipmentOrderConfirmViewModel

{
    //private ObservableCollection<DynamicInventoryViewModel> _selectedForOrder;
    public IEnumerable<DynamicInventoryViewModel> SelectedForOrder { get; }
    public DEquipmentOrderConfirmViewModel(IEnumerable<DynamicInventoryViewModel> selectedForOrder)
    {
        SelectedForOrder = selectedForOrder;
    }
}