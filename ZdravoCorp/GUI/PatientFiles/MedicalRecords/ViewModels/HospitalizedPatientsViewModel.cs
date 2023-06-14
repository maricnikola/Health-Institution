using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Autofac;
using ZdravoCorp.Core.PatientFiles.Refferals.Models;
using ZdravoCorp.Core.PatientFiles.Refferals.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.GUI.Main;
using ZdravoCorp.GUI.PatientFiles.MedicalRecords.Views;
using ZdravoCorp.GUI.PatientFiles.Refferals.ViewModels;

namespace ZdravoCorp.GUI.PatientFiles.MedicalRecords.ViewModels;

public class HospitalizedPatientsViewModel: ViewModelBase
{
    public ICommand ChangeTherapyCommand { get; private set; }
    public ObservableCollection<HospitalRefferalViewModel> HospitalRefferals { get; }
    private IHospitalRefferalService _hospitalRefferalService;
    public HospitalRefferalViewModel SelectedHospitalRefferal { get; set; }
    public  HospitalizedPatientsViewModel()
    {
        _hospitalRefferalService = Injector.Container.Resolve<IHospitalRefferalService>();
        HospitalRefferals = new ObservableCollection<HospitalRefferalViewModel>();
        ShowHospitalizedPatients();
        ChangeTherapyCommand = new DelegateCommand(o => OpenChangeDialog());
    }
    public void ShowHospitalizedPatients()
    {
        HospitalRefferals.Clear();
        foreach (HospitalRefferal hospitalRefferal in _hospitalRefferalService.GetAll())
        {
            if(hospitalRefferal.Time.End.Date > DateTime.Now.Date)
                HospitalRefferals.Add(new HospitalRefferalViewModel(hospitalRefferal));
        }
    }
    public void OpenChangeDialog()
    {
        if(SelectedHospitalRefferal != null)
        {
            var dialog = new ChangeHospitalTreatmentView() { DataContext = new ChangeHospitalTreatmentViewModel(_hospitalRefferalService,SelectedHospitalRefferal.Id,this) };
            dialog.Show();
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }
}
