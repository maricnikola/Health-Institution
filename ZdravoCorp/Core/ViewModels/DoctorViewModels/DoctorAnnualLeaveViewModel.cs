using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Services.AnnualLeaveServices;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class DoctorAnnualLeaveViewModel : ViewModelBase
{
    public ICommand CreateAnnualLeaveCommand { get; private set; }
	private IAnnualLeaveService _annualLeaveService;
    public DoctorAnnualLeaveViewModel(IAnnualLeaveService annualLeaveService)
    {
		_annualLeaveService = annualLeaveService;

    }
	private DateTime _startDate;
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
	private DateTime _endDate;
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
}
