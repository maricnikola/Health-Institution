using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Models;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Services;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.GUI.HospitalSystem.AnnualLeaves.Commands;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.AnnualLeaves.ViewModels;

public class DoctorAnnualLeaveViewModel : ViewModelBase
{
    public ICommand SubmitAnnualLeaveCommand { get; set; }
	public ICommand SubmitCancelCommand { get; private set; }
	private IAnnualLeaveService _annualLeaveService;
	private IScheduleService _scheduleService;
	public ObservableCollection<AnnualLeaveRequestViewModel> AnnualLeaves { get; }

	private string _doctorMail;
    public DoctorAnnualLeaveViewModel(IAnnualLeaveService annualLeaveService,IScheduleService scheduleService,string doctorMail)
    {
		AnnualLeaves = new ObservableCollection<AnnualLeaveRequestViewModel>();
		_scheduleService = scheduleService;
		_doctorMail= doctorMail;
		_annualLeaveService = annualLeaveService;
		ShowAnnualLeaves();
		SubmitAnnualLeaveCommand = new CreateAnnualLeaveCommand(this, _annualLeaveService, _scheduleService,_doctorMail);

		SubmitCancelCommand = new CancelAnnualLeaveCommand(this, _annualLeaveService);

    }
	public void ShowAnnualLeaves()
	{
		AnnualLeaves.Clear();
        foreach (AnnualLeave annualLeave in _annualLeaveService.GetAll())
        {
            if (annualLeave.RequestStatus.Equals(AnnualLeave.Status.Denied)) continue;
            AnnualLeaves.Add(new AnnualLeaveRequestViewModel(annualLeave));
        }
    }
	private DateTime _startDate =  DateTime.Now + TimeSpan.FromHours(1);
	public DateTime StartDate
	{
		get
		{
			return _startDate;
		}
		set
		{
			_startDate = value;
			OnPropertyChanged(nameof(StartDate));
		}
	}
	private DateTime _endDate = DateTime.Now + TimeSpan.FromHours(1);
	public DateTime EndDate
	{
		get
		{
			return _endDate;
		}
		set
		{
			_endDate = value;
			OnPropertyChanged(nameof(EndDate));
		}
	}

	private string _reasonInput;
	public string ReasonInput
	{
		get
		{
			return _reasonInput;
		}
		set
		{
			_reasonInput = value;
			OnPropertyChanged(nameof(ReasonInput));
		}
	}
    private AnnualLeaveRequestViewModel _selectedAnnualLeave;
    public AnnualLeaveRequestViewModel SelectedAnnualLeave
    {
        get
        {
            return _selectedAnnualLeave;
        }
        set
        {
            _selectedAnnualLeave = value;
            OnPropertyChanged(nameof(SelectedAnnualLeave));
        }
    }
	public void ResetValues()
	{
		StartDate = DateTime.Now + TimeSpan.FromHours(1);
		EndDate = DateTime.Now + TimeSpan.FromHours(1);
		ReasonInput = "";
    }
}
