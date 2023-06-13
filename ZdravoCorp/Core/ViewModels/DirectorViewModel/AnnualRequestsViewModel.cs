﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Services.AnnualLeaveServices;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class AnnualRequestsViewModel : ViewModelBase
{
    private IAnnualLeaveService _annualLeaveService;
    private ObservableCollection<AnnualLeaveRequestViewModel> _requests;
    
    public AnnualLeaveRequestViewModel? SelectedRequest { get; set; }
    
    public ICommand ApproveAnnualRequestCommand { get; set; }
    public ICommand DenyAnnualRequestCommand { get; set; }
    public IEnumerable<AnnualLeaveRequestViewModel> Requests
    {
        get => _requests;
        set
        {
            _requests = new ObservableCollection<AnnualLeaveRequestViewModel>(value);
            OnPropertyChanged();
        }
    }
    
    public AnnualRequestsViewModel(IAnnualLeaveService annualLeaveService)
    {
        _annualLeaveService = annualLeaveService;
        ApproveAnnualRequestCommand = new DelegateCommand(o => ApproveRequest(), o => CanApprove());
        DenyAnnualRequestCommand = new DelegateCommand(o => DenyRequest(), o => CanDeny());
    }

    private void ApproveRequest()
    {
        
    }

    private void DenyRequest()
    {
        _annualLeaveService.Deny(SelectedRequest.Id);
    }

    private bool CanDeny()
    {
        return SelectedRequest != null && SelectedRequest.Status == "Denied";
    }
    private bool CanApprove()
    {
        return SelectedRequest != null && SelectedRequest.Status == "Approved";
    }
}