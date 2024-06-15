using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.PatientFiles.Refferals.Models;
using ZdravoCorp.Core.PatientFiles.Refferals.Services;
using ZdravoCorp.GUI.Main;
using ZdravoCorp.GUI.Scheduling.ViewModels;
using ZdravoCorp.GUI.Scheduling.Views;

namespace ZdravoCorp.GUI.PatientFiles.MedicalRecords.ViewModels;

public class ChangeHospitalTreatmentViewModel: ViewModelBase
{
	
    private IHospitalRefferalService _hospitalRefferalService;
	private HospitalRefferal _hospitalRefferal;
	public ObservableCollection<TherapyViewModel> Therapies { get; }
	private HospitalizedPatientsViewModel _hospitalizedPatientsViewModel;
    public ICommand AddNewTherapyCommand { get; private set; }
	public ICommand ChangeEndDateCommand { get; private set; }
    public ChangeHospitalTreatmentViewModel(IHospitalRefferalService hospitalRefferalService,int id,
		HospitalizedPatientsViewModel hospitalizedPatientsViewModel)
    {
		_hospitalizedPatientsViewModel = hospitalizedPatientsViewModel;
		Therapies = new ObservableCollection<TherapyViewModel>();
        _hospitalRefferalService = hospitalRefferalService;
		_hospitalRefferal = _hospitalRefferalService.GetById(id);
		foreach(Therapy therapy in _hospitalRefferal.InitialTherapy)
		{
			Therapies.Add(new TherapyViewModel(therapy));
		}
		_newEndDate = _hospitalRefferal.Time.End;
		AddNewTherapyCommand = new DelegateCommand(o => AddNewTherapy());
		ChangeEndDateCommand = new DelegateCommand(o => ChangeEndDate()); 

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
			if (therapyDescription.Length < 4)
			{
                MessageBox.Show("Invalid data for therapy", "Error", MessageBoxButton.OK);
				return;
            }
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
    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }
    public void ChangeEndDate()
	{
		var EndDate = NewEndDate;
		var now = DateTime.Now;
		if (!_hospitalRefferalService.UpdateEndDate(_hospitalRefferal.Id, EndDate))
		{
            MessageBox.Show("Invalid date for changes", "Error", MessageBoxButton.OK);
			return;
        }
		_hospitalizedPatientsViewModel.ShowHospitalizedPatients();
        CloseWindow();
		if(EndDate.Date == now.Date)
			OpenControlAppointment();
		
    }
	public void OpenControlAppointment()
	{
		var dialog = new ControlAppointmentView() { DataContext = new ControlAppointmentViewModel(_hospitalRefferalService, _hospitalRefferal,_hospitalizedPatientsViewModel) };
		dialog.Show();
	}
}
