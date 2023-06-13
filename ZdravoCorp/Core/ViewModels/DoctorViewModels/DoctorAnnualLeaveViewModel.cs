using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.AnnualLeaves;
using ZdravoCorp.Core.Services.AnnualLeaveServices;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.ViewModels.DirectorViewModel;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class DoctorAnnualLeaveViewModel : ViewModelBase
{
    public ICommand CreateAnnualLeaveCommand { get; private set; }
	private IAnnualLeaveService _annualLeaveService;
	public ObservableCollection<AnnualLeaveRequestViewModel> AnnualLeaves;
	private string _doctorMail;
    public DoctorAnnualLeaveViewModel(IAnnualLeaveService annualLeaveService,string doctorMail)
    {
		_annualLeaveService = annualLeaveService;
		foreach(AnnualLeave annualLeave in _annualLeaveService.GetAll())
		{
			AnnualLeaves.Add(new AnnualLeaveRequestViewModel(annualLeave));
		}
		CreateAnnualLeaveCommand = new DelegateCommand(o => CreateAnnualLeaveRequest());

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

	private string _reason;
	public string Reason
	{
		get
		{
			return _reason;
		}
		set
		{
			_reason = value;
			OnPropertyChanged(nameof(Reason));
		}
	}

	public void CreateAnnualLeaveRequest()
	{
		string reason = Reason;
		DateTime startDate = StartDate;
		DateTime endDate = EndDate;
		TimeSlot time = new TimeSlot(startDate, endDate);
		AnnualLeaveDTO annualLeave = new AnnualLeaveDTO(reason, time, _doctorMail);
		AnnualLeaveDTO addedAnnualLeave =  _annualLeaveService.AddAnnualLeave(annualLeave);
		if(addedAnnualLeave == null)
		{
            MessageBox.Show("Invalid data for Annual leave request!", "Error", MessageBoxButton.OK);
			return;
        }
		AnnualLeaves.Add(new AnnualLeaveRequestViewModel(new AnnualLeave(annualLeave.Reason,annualLeave.Time,annualLeave.Id,annualLeave.DoctorMail,annualLeave.RequestStatus)));
	}
}
