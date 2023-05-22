using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.AnamnesisReport;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Services.PatientServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.ViewModels.NurseViewModel;

public class PatientReceptionViewModel : ViewModelBase
{
    private string _alergens;

    private string _deseaseHistory;

    private string _patientEmail;
    private readonly IPatientService _patientService;
    private readonly IScheduleService _scheduleService;

    private string _sympthomes;


    public PatientReceptionViewModel(IPatientService patientService, IScheduleService scheduleService)
    {
        /*this._scheduleRepository = new ScheduleRepository();
        this._scheduleRepository.LoadAppointments();*/
        _patientService = patientService;
        _scheduleService = scheduleService;
        //_patientEmail = "nesto@gmail.com";

        SubmitButton_OnClick = new DelegateCommand(o => AddAnamnesis());
    }

    public string PatientEmail
    {
        get => _patientEmail;
        set
        {
            _patientEmail = value;
            OnPropertyChanged();
        }
    }

    public string Alergens
    {
        get => _alergens;
        set
        {
            _alergens = value;
            OnPropertyChanged();
        }
    }

    public string Sympthomes
    {
        get => _sympthomes;
        set
        {
            _sympthomes = value;
            OnPropertyChanged(nameof(Alergens));
        }
    }

    public string DeseaseHistory
    {
        get => _deseaseHistory;
        set
        {
            _deseaseHistory = value;
            OnPropertyChanged();
        }
    }


    public ICommand SubmitButton_OnClick { get; set; }

    public void AddAnamnesis()
    {
        var pomoc = _patientService.GetByEmail(_patientEmail);
        if (_patientService.GetByEmail(_patientEmail) != null)
        {
            var sympthomeList = _sympthomes.Split(", ").ToList();
            var alergensList = _alergens.Split(", ").ToList();
            var anamnesis = new Anamnesis(sympthomeList, "doctors oppinion", "keyword", alergensList);
            //negde poslati ovu anamnezu...
            var interval = new TimeSlot(DateTime.Now, DateTime.Now.AddMinutes(15));
            var appointment = _scheduleService.GetPatientsFirstAppointment(_patientEmail, interval);
            if (appointment != null)
            {
                appointment.Anamnesis = anamnesis;
                MessageBox.Show("Anamnesis submited");
            }
            else
            {
                MessageBox.Show("Too early for termin");
            }
        }
        else
        {
            MessageBox.Show("Invalid username");
        }
        // _scheduleRepository. neka funkcija koja ce vracati da li pacijent ima pregled zakazan u narednih 15 minuta
        // public Appointment HasPatientAppointmentInNext15(string email)
        //ovde uraditi da ubacuje anamnezu i sta vec
    }


    /*private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
    {
        
    }*/

    //neka fuhnkcija 
}