using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.SpecialistsRefferals;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Services.SpecialistsRefferalServices;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class AddSpecialistsRefferalViewModel : ViewModelBase
{
    private PerformAppointmentViewModel _performAppointmentViewModel;
    public ICommand CreateRefferal { get; }
    public ICommand SpecialisationComboEnable { get; }
    public ICommand DoctorComboEnable { get; }
 
    private List<DoctorViewModel> _doctors;
    private Doctor _doctor;
    private Patient _patient;
    private IDoctorService _doctorService;
    private ISpecialistsRefferalService _specialistsRefferalService;
    private IScheduleService _scheduleService;
    private Appointment _appointment;
    private IInventoryService _inventoryService;
    private int _roomId;
    public AddSpecialistsRefferalViewModel(PerformAppointmentViewModel performAppointmentViewModel,
        IDoctorService doctorService,Doctor doctorPerforming,Patient patient,IScheduleService scheduleService,
        Appointment performingAppointment)
    {
 
        _patient = patient;
        _appointment = performingAppointment;
        _scheduleService = scheduleService;
        _specialistsRefferalService = Injector.Container.Resolve<ISpecialistsRefferalService>();
        _doctorService = doctorService;
        List<String> doctorNamesList = new List<string>();
        List<String> specialistsNamesList = new List<string>();
        _doctor = doctorPerforming;
        foreach(Doctor doctor in _doctorService.GetAll())
        {
            if (doctor.Equals(_doctor)) continue;
            doctorNamesList.Add(doctor.FullName);
            specialistsNamesList.Add(doctor.Specialization.ToString());
        }
        specialistsNamesList = specialistsNamesList.Distinct().ToList();
        DoctorNames = doctorNamesList.ToArray();
        SpecialisationsNames = specialistsNamesList.ToArray();

        _performAppointmentViewModel = performAppointmentViewModel;

        CloseDialog = new DelegateCommand(o => CloseWindow(true));
        DoctorComboEnable = new DelegateCommand(o => EnableDoctorsCombo());
        SpecialisationComboEnable = new DelegateCommand(o => EnableSpecialisationsCombo());
        CreateRefferal = new DelegateCommand(o => CreateSpecialistsRefferal());
    }

    public string[] SpecialisationsNames { get; set; }
    public string[] DoctorNames { get; set; }
    public ICommand CloseDialog { get; }

    private string _selectedDoctor;
    public string SelectedDoctor
    {
        get
        {
            return _selectedDoctor;
        }
        set
        {
            _selectedDoctor = value;
            OnPropertyChanged(nameof(SelectedDoctor));
        }
    }
    private string _selectedSpecialisation;
    public string SelectedSpecialisation
    {
        get
        {
            return _selectedSpecialisation;
        }
        set
        {
            _selectedSpecialisation = value;
            OnPropertyChanged(nameof(SelectedSpecialisation));
        }
    }

    private bool _isDoctorEnabled = false;
    public bool IsDoctorEnabled
    {
        get { return _isDoctorEnabled; }
        set
        {
            _isDoctorEnabled = value;
            OnPropertyChanged(nameof(IsDoctorEnabled));
        }
    }
    private bool _isSpecialisationsEnabled = false;
    public bool IsSpecialisationsEnabled
    {
        get { return _isSpecialisationsEnabled; }
        set
        {
            _isSpecialisationsEnabled = value;
            OnPropertyChanged(nameof(IsSpecialisationsEnabled));
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
    private void EnableDoctorsCombo()
    {
        SelectedSpecialisation = null;
        IsSpecialisationsEnabled = false;
        IsDoctorEnabled = true;
    }
    private void EnableSpecialisationsCombo()
    {
        IsDoctorEnabled= false;
        IsSpecialisationsEnabled = true;
        SelectedDoctor = null;
    }

    private void CreateSpecialistsRefferal()
    {
        if (checkSelectedData())
        {
            MessageBox.Show("Invalid data for performing", "Error", MessageBoxButton.OK);
            return;
        }
        if(CheckIfRefferalExists())
        {
            MessageBox.Show("Specialist refferal already exists!", "Error", MessageBoxButton.OK);
            return;
        }
        if (IsDoctorEnabled)
        {
            foreach(var doctor in _doctorService.GetAll())
            {
                if (doctor.FullName.Equals(SelectedDoctor))
                {
                    _specialistsRefferalService.AddRefferal(new SpecialistsRefferal(IDGenerator.GetId(), _patient.Email, doctor.Email));
                }
            }
        }
        if (IsSpecialisationsEnabled)
        {
            foreach(var doctor in _doctorService.GetAll())
            {
                if (doctor.Email.Equals(_doctor.Email)) continue;
                if(doctor.Specialization.ToString().Equals(SelectedSpecialisation))
                    _specialistsRefferalService.AddRefferal(new SpecialistsRefferal(IDGenerator.GetId(),_patient.Email,doctor.Email));
            }
        }
        var appointmentDto = new AppointmentDTO(_appointment.Id, _appointment.Time.Start, _appointment.Doctor,
        _appointment.PatientEmail, null);
        _scheduleService.CancelAppointmentByDoctor(appointmentDto);
        CloseWindow(true);

    }

    private bool checkSelectedData()
    {
        return (!IsDoctorEnabled && !IsSpecialisationsEnabled) || (IsDoctorEnabled && SelectedDoctor == null) 
            || (IsSpecialisationsEnabled && SelectedSpecialisation == null);
    }
    private bool CheckIfRefferalExists()
    {
        List<string> doctorMails = new List<string>();
        foreach(Doctor doctor in _doctorService.GetAll())
        {
            if (doctor.FullName.Equals(SelectedDoctor) || doctor.Specialization.ToString().Equals(SelectedSpecialisation))
            {
                doctorMails.Add(doctor.Email);
            }
            
        }
        foreach(SpecialistsRefferal refferal in _specialistsRefferalService.GetAll())
        {
            if (_appointment.PatientEmail.Equals(refferal.PatientMail) && doctorMails.Contains(refferal.DoctorMail)) return true;
        }
        return false;
    }
}
