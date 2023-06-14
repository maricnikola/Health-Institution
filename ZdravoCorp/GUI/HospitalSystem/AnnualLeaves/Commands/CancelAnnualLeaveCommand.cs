using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Models;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Services;
using ZdravoCorp.GUI.HospitalSystem.AnnualLeaves.ViewModels;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.AnnualLeaves.Commands;

public class CancelAnnualLeaveCommand : CommandBase
{
    private IAnnualLeaveService _annualLeaveService;
    private DoctorAnnualLeaveViewModel _doctorAnnualLeaveViewModel;
    private AnnualLeaveRequestViewModel _selectedAnnualLeave => _doctorAnnualLeaveViewModel.SelectedAnnualLeave;
    public CancelAnnualLeaveCommand(DoctorAnnualLeaveViewModel doctorAnnualLeaveViewModel,IAnnualLeaveService annualLeaveService)
    {
        _doctorAnnualLeaveViewModel = doctorAnnualLeaveViewModel;
        _doctorAnnualLeaveViewModel.PropertyChanged += OnViewModelPropertyChanged;
        _annualLeaveService = annualLeaveService;
    }


    public override bool CanExecute(object? parameter)
    {
        if (_selectedAnnualLeave == null) return false;
        if (!_annualLeaveService.CheckAnnualLeaveForDeny(_annualLeaveService.GetById(_selectedAnnualLeave.Id))) return false;
        return true;
    }


    public override void Execute(object? parameter)
    {
        AnnualLeave annualLeave = _annualLeaveService.GetById(_selectedAnnualLeave.Id);
        _annualLeaveService.DenyByDoctor(_selectedAnnualLeave.Id);
        _doctorAnnualLeaveViewModel.ShowAnnualLeaves();
        
    }
    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_doctorAnnualLeaveViewModel.SelectedAnnualLeave) )
        {
            OnCanExecutedChanged();
        }
    }
}
