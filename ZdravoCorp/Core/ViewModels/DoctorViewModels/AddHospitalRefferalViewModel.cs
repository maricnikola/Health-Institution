using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.HospitalRefferals;
using ZdravoCorp.Core.Services.HospitalRefferalServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Services.SpecialistsRefferalServices;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class AddHospitalRefferalViewModel: ViewModelBase
{

    private PerformAppointmentViewModel _performAppointmentViewModel;

    public ICommand Close { get; }
    public ICommand CreateRefferal { get; }
    public IHospitalRefferalService _hospitalRefferalService;
    public IScheduleService _scheduleService;
    private Appointment _appointment;
    public AddHospitalRefferalViewModel(PerformAppointmentViewModel performAppointmentViewModel,Appointment appointment,
        IScheduleService scheduleService,IHospitalRefferalService hospitalRefferalService)
    {
        _appointment = appointment;
        _scheduleService = scheduleService;
        _hospitalRefferalService = hospitalRefferalService;
        _performAppointmentViewModel = performAppointmentViewModel;
        Close = new DelegateCommand(o => CloseWindow(true));
        CreateRefferal = new DelegateCommand(o => CreateHospitalRefferal());
    }

    private int _duration;
    public int Duration
    {
        get
        {
            return _duration;
        }
        set
        {
            _duration = value;
            OnPropertyChanged(nameof(Duration));
        }
    }
    private string _initialTherapy;
    public string InitialTherapy
    {
        get
        {
            return _initialTherapy;
        }
        set
        {
            _initialTherapy = value;
            OnPropertyChanged(nameof(InitialTherapy));
        }
    }
    private string _additionTests;
    public string AdditionTests
    {
        get
        {
            return _additionTests;
        }
        set
        {
            _additionTests = value;
            OnPropertyChanged(nameof(AdditionTests));
        }
    }
    private void CloseWindow(bool backToPerform)
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
        if (backToPerform)
        {
            var performWindow = new PerformAppointmentView() { DataContext = _performAppointmentViewModel };
            performWindow.Show();
        }
    }
    private void CreateHospitalRefferal()
    {
        try
        {
            int duration = Duration;
            TimeSlot time = new TimeSlot(_appointment.Time.End, _appointment.Time.End.AddDays(duration));
            string initialTherapy = InitialTherapy;
            if(initialTherapy.Length < 5)
            {
                MessageBox.Show("Invalid data for hospital refferal", "Error", MessageBoxButton.OK);
                return;
            }
            string additionTests = AdditionTests;
            _hospitalRefferalService.AddRefferal(new HospitalRefferal(IDGenerator.GetId(),_appointment.PatientEmail,time, initialTherapy, additionTests));
            CloseWindow(false);
            _performAppointmentViewModel.ShowDEquipmentSpentDialog();
            var appointmentDto = new AppointmentDTO(_appointment.Id, _appointment.Time.Start, _appointment.Doctor,
            _appointment.PatientEmail, null);
            _scheduleService.CancelAppointmentByDoctor(appointmentDto);
        }
        catch (Exception)
        {

            MessageBox.Show("Invalid data for hospital refferal", "Error", MessageBoxButton.OK);
        }
    }
}
