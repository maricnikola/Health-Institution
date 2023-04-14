using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ZdravoCorp.Core.ViewModels;

public class AddAppointmentViewModel: ViewModelBase
{
	private String _username;
	public String Username
	{
		get
		{
			return _username;
		}
		set
		{
			_username = value;
			OnPropertyChanged(nameof(Username));
		}
	}
	private String _startTime;
	public String StartTime
	{
		get
		{
			return _startTime;
		}
		set
		{
			_startTime = value;
			OnPropertyChanged(nameof(StartTime));
		}
	}

	public ICommand AddCommand{ get;  }	
	public ICommand CancelCommand { get; }
	
}
