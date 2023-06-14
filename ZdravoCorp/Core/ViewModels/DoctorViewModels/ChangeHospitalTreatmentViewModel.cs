using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.HospitalRefferals;
using ZdravoCorp.Core.Models.Therapies;
using ZdravoCorp.Core.Services.HospitalRefferalServices;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class ChangeHospitalTreatmentViewModel: ViewModelBase
{
	
    private IHospitalRefferalService _hospitalRefferalService;
	private HospitalRefferal _hospitalRefferal;
	public ObservableCollection<TherapyViewModel> Therapies { get; }
    public ICommand AddNewTherapyCommand { get; private set; }
    public ChangeHospitalTreatmentViewModel(IHospitalRefferalService hospitalRefferalService,int id)
    {
		Therapies = new ObservableCollection<TherapyViewModel>();
        _hospitalRefferalService = hospitalRefferalService;
		_hospitalRefferal = _hospitalRefferalService.GetById(id);
		foreach(Therapy therapy in _hospitalRefferal.InitialTherapy)
		{
			Therapies.Add(new TherapyViewModel(therapy));
		}
		AddNewTherapyCommand = new DelegateCommand(o => AddNewTherapy());

    }

	private string _newTherapy;
	public string NewTherapy
	{
		get
		{
			return _newTherapy;
		}
		set
		{
			_newTherapy = value;
			OnPropertyChanged(nameof(NewTherapy));
		}
	}
	private DateTime _newEndDate;
	public DateTime NewEndDate
	{
		get
		{
			return _newEndDate;
		}
		set
		{
			_newEndDate = value;
			OnPropertyChanged(nameof(NewEndDate));
		}
	}
	public void AddNewTherapy()
	{
		try
		{
			var therapyDescription = NewTherapy;
			Therapy therapy = new Therapy(therapyDescription, true);
			_hospitalRefferalService.AddNewTherapy(therapy, _hospitalRefferal.Id);
			Therapies.Clear();
            foreach (Therapy therapyUpdate in _hospitalRefferal.InitialTherapy)
            {
                Therapies.Add(new TherapyViewModel(therapyUpdate));
            }
        }
        catch (Exception)
		{
            MessageBox.Show("Invalid data for therapy", "Error", MessageBoxButton.OK);
        }
    }
}
