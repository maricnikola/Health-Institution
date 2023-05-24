using System;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Services.RenovationServices;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class ScheduleRenovationWindowViewModel
{
    private IRenovationService _renovationService;
    public event EventHandler OnRequestClose;
    public ICommand ConfirmRenovation { get; }
    public ICommand CancelRenovation { get; }
    
    
    public ScheduleRenovationWindowViewModel(IRenovationService renovationService, int roomId)
    {
        _renovationService = renovationService;
        ConfirmRenovation = new DelegateCommand(o => Confirm(), o => CanConfirm());
        CancelRenovation = new DelegateCommand(o => Cancel());
    }

    private void Cancel()
    {
        OnRequestClose?.Invoke(this, new EventArgs());
    }
    private void Confirm(){}

    private bool CanConfirm()
    {
        return true;
    }



}