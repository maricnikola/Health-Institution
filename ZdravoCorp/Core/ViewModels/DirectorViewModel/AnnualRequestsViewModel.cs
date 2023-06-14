using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Services.AnnualLeaveServices;
using ZdravoCorp.Core.Services.NotificationServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.View.DirectorView;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class AnnualRequestsViewModel : ViewModelBase
{
    private IAnnualLeaveService _annualLeaveService;
    private IScheduleService _scheduleService;
    private INotificationService _notificationService;
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
    
    public AnnualRequestsViewModel(IAnnualLeaveService annualLeaveService, IScheduleService scheduleService, INotificationService notificationService)
    {
        _annualLeaveService = annualLeaveService;
        _scheduleService = scheduleService;
        _notificationService = notificationService;
        _requests = new ObservableCollection<AnnualLeaveRequestViewModel>();
        ApproveAnnualRequestCommand = new DelegateCommand(o => ApproveRequest(), o => CanApprove());
        DenyAnnualRequestCommand = new DelegateCommand(o => DenyRequest(), o => CanDeny());
        _annualLeaveService.DataChanged += (o, e) => PopulateTable();
        PopulateTable();
    }

    private void PopulateTable()
    {
        _requests = new ObservableCollection<AnnualLeaveRequestViewModel>();
        foreach (var request in _annualLeaveService.GetAll())
        {
            _requests.Add(new AnnualLeaveRequestViewModel(request));
        }
    }
    private void ApproveRequest()
    {
        var vm = new ApproveAnnualRequestViewModel(_scheduleService, _annualLeaveService,_annualLeaveService.GetById(SelectedRequest.Id), _notificationService);
        var window = new ApproveAnnualRequestView() { DataContext = vm };
        vm.OnRequestClose += (s, e) => window.Close();
        window.Show();
    }

    private void DenyRequest()
    {
        _annualLeaveService.Deny(SelectedRequest.Id);
    }

    private bool CanDeny()
    {
        return SelectedRequest != null && SelectedRequest.Status == "Pending";
    }
    private bool CanApprove()
    {
        return SelectedRequest != null && SelectedRequest.Status == "Pending";
    }
}