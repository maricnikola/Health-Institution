using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Models;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Services;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.GUI.HospitalSystem.AnnualLeaves.ViewModels;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.AnnualLeaves.Commands;

public class CreateAnnualLeaveCommand : CommandBase
{
    private DoctorAnnualLeaveViewModel _doctorAnnualLeaveViewModel;
    private IAnnualLeaveService _annualLeaveService;
    private IScheduleService _scheduleService;
    private string _doctorMail;
    private TimeSlot _time => new TimeSlot(_doctorAnnualLeaveViewModel.StartDate,_doctorAnnualLeaveViewModel.EndDate);
    private string _reason => _doctorAnnualLeaveViewModel.ReasonInput;
    public CreateAnnualLeaveCommand(DoctorAnnualLeaveViewModel doctorAnnualLeaveViewModel, IAnnualLeaveService annualLeaveService,
        IScheduleService scheduleService,string doctorMail)
    {
        _doctorMail = doctorMail;
        _doctorAnnualLeaveViewModel = doctorAnnualLeaveViewModel;
        _doctorAnnualLeaveViewModel.PropertyChanged += OnViewModelPropertyChanged;
        _annualLeaveService = annualLeaveService;
        _scheduleService = scheduleService;

    }

    public override bool CanExecute(object? parameter)
    {
        try
        {
            if (!_scheduleService.IsDoctorAvailable(_time, _doctorMail)) return false;
            if (!_annualLeaveService.CheckAnnualLeaveData(_reason,_time)) return false;
        }
        catch (Exception)
        {
            return false ;
        }
        return true;
    }

    public override void Execute(object? parameter)
    {
        AnnualLeaveDTO annualLeave = new AnnualLeaveDTO(_reason, _time, _doctorMail);
        _annualLeaveService.AddAnnualLeave(annualLeave);
        _doctorAnnualLeaveViewModel.ShowAnnualLeaves();
        _doctorAnnualLeaveViewModel.ResetValues();
    }
 
    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_doctorAnnualLeaveViewModel.StartDate) ||
            e.PropertyName == nameof(_doctorAnnualLeaveViewModel.EndDate) ||
            e.PropertyName == nameof(_doctorAnnualLeaveViewModel.ReasonInput))
        {
            OnCanExecutedChanged();
        }
    }
}
