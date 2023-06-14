using System;
using System.ComponentModel;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Services;
using ZdravoCorp.Core.HospitalSystem.Notifications.Services;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;
using ZdravoCorp.GUI.HospitalSystem.AnnualLeaves.ViewModels;
using ZdravoCorp.GUI.HospitalSystem.AnnualLeaves.Views;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.AnnualLeaves.Commands;

public class ApproveAnnualRequestCommand : CommandBase
{

    private IScheduleService _scheduleService;
    private IAnnualLeaveService _annualLeaveService;
    private INotificationService _notificationService;
    private AnnualLeaveRequestViewModel? SelectedRequest;
    private AnnualRequestsViewModel _annualRequestsViewModel;

    public ApproveAnnualRequestCommand(AnnualRequestsViewModel annualRequestsViewModel, IScheduleService scheduleService, IAnnualLeaveService annualLeaveService, INotificationService notificationService, AnnualLeaveRequestViewModel selectedRequest)
    {
        _annualRequestsViewModel = annualRequestsViewModel;
        _scheduleService = scheduleService;
        _annualLeaveService = annualLeaveService;
        _notificationService = notificationService;
        SelectedRequest = selectedRequest;
        _annualRequestsViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }
    public override void Execute(object? parameter)
    {
        var vm = new ApproveAnnualRequestViewModel(_scheduleService, _annualLeaveService,_annualLeaveService.GetById(SelectedRequest.Id), _notificationService);
        var window = new ApproveAnnualRequestView() { DataContext = vm };
        vm.OnRequestClose += (s, e) => window.Close();
        window.Show();
    }
    
    public override bool CanExecute(object? parameter)
    {
        return SelectedRequest != null && SelectedRequest.Status == "Pending";
    }
    
    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_annualRequestsViewModel.SelectedRequest))
        {
            OnCanExecutedChanged();
        }
    }
}