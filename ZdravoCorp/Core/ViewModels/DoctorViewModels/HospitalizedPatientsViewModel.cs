using Autofac;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Models.HospitalRefferals;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.HospitalRefferalServices;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class HospitalizedPatientsViewModel
{
    public ICommand ChangeTherapyCommand { get; private set; }
    public ObservableCollection<HospitalRefferalViewModel> HospitalRefferals { get; }
    private IHospitalRefferalService _hospitalRefferalService;
    public HospitalRefferalViewModel SelectedHospitalRefferal { get; set; }
    public  HospitalizedPatientsViewModel()
    {
        _hospitalRefferalService = Injector.Container.Resolve<IHospitalRefferalService>();
        HospitalRefferals = new ObservableCollection<HospitalRefferalViewModel>();
        foreach(HospitalRefferal hospitalRefferal in _hospitalRefferalService.GetAll())
        {
            HospitalRefferals.Add(new HospitalRefferalViewModel(hospitalRefferal));
        }
    }
}
