using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Models.User;

namespace ZdravoCorp.Core.ViewModels;

public class AddAppointmentViewModel: ViewModelBase
{
	private String _username;
	private ObservableCollection<String> _patientsFullname { get; }
	public IEnumerable<String> Patients => _patientsFullname;

	public AddAppointmentViewModel(List<Patient> patients )
	{
		_patientsFullname = new ObservableCollection<string>();
		foreach(Patient p in patients)
		{
			_patientsFullname.Add(p.FullName);
		}
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

	private int _startTimeHours;
	public int StartTimeHours
	{
		get
		{
			return _startTimeHours;
		}
		set
		{
			_startTimeHours = value;
			OnPropertyChanged(nameof(StartTimeHours));
		}
	}
	private int _startTimeMinutes;
	public int StartTimeMinutes
	{
		get
		{
			return _startTimeMinutes;
		}
		set
		{
			_startTimeMinutes = value;
			OnPropertyChanged(nameof(StartTimeMinutes));
		}
	}



	public ICommand AddCommand{ get;  }	
	public ICommand CancelCommand { get; }
	
}
