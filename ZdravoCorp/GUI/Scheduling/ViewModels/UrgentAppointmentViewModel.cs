using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.PatientFiles.MedicalRecords.Services;
using ZdravoCorp.Core.Scheduling.Models;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.Scheduling.ViewModels;

public class UrgentAppointmentViewModel : ViewModelBase
{
    private IDoctorService _doctorService;
    private IMedicalRecordService _medicalRecordService;
    private IScheduleService _scheduleService;

    public UrgentAppointmentViewModel(IMedicalRecordService medicalRecordService,
        IScheduleService scheduleService, IDoctorService doctorService)
    {
        _medicalRecordService = medicalRecordService;
        _doctorService = doctorService;
        _scheduleService = scheduleService;
        FindUrgentAppointmentCommand = new DelegateCommand(o => FindUrgentAppointment());
        SpecializationTypes = new ObservableCollection<string>();
        SpecializationTypes.Add(Doctor.SpecializationType.Surgeon.ToString());
        SpecializationTypes.Add(Doctor.SpecializationType.Psychologist.ToString());
        SpecializationTypes.Add(Doctor.SpecializationType.Neurologist.ToString());
        SpecializationTypes.Add(Doctor.SpecializationType.Urologist.ToString());
        SpecializationTypes.Add(Doctor.SpecializationType.Anesthesiologist.ToString());
        //_specializationTypes.Insert("Any");
    }

    public ICommand FindUrgentAppointmentCommand { get; set; }

    public string PatientEmail { get; set; }

    public Doctor.SpecializationType SpecializationType { get; set; }

    public ObservableCollection<string> SpecializationTypes { get; }

    public void FindUrgentAppointment()
    {
        /*DateTime now = DateTime.Now;
        List<Doctor> suitableDoctors = _doctorRepository.GetAllSpecialized(_specializationType);
        if (suitableDoctors.Count == 0)
        {
            //napraviti pop up obavestenja da nema doktora za ovaj posao
        }
        else
        {
            List<List<Appointment>> allDoctorsAppointments = new List<List<Appointment>> { };
            List<Appointment> appointments = new List<Appointment> { };
            List<Tuple<TimeSlot, string>> termins = new List<Tuple<TimeSlot, string>> { }; 
            //List<string> doctorsEmails = new List<string>();
            DateTime latestTime = DateTime.Now.AddHours(2);

            TimeSlot interval = new TimeSlot(now, now.AddHours(2));
            foreach (Doctor suitableDoctor in suitableDoctors)
            {
                //ovde pitati kuma sta kako ako je ikako moguce private/public kod metode mu smeta
                //TimeSlot compareTime = _scheduleRepository.FindAvailableTimeslotsForOneDoctor(suitableDoctor.Email, interval, now, null);
                TimeSlot termin = _scheduleRepository.FindAvailableTimeslotsForOneDoctor(suitableDoctor.Email, interval, DateTime.Today);
                if (termin != null) {
                    termins.Insert(new Tuple<TimeSlot, string>(termin, suitableDoctor.Email));
                }
                termins.Sort((a, b) => a.Item1.IsBefore(b.Item1));

                //allDoctorsAppointments.Insert(_scheduleRepository.GetDoctorAppointments(suitableDoctor.Email));
            }



        }*/
    }

    public DateTime GetFirstTime(List<List<Appointment>> allDoctorsAppointments)
    {
        var latestTime = DateTime.Now.AddHours(2);
        var earliestTime = DateTime.Now;

        foreach (var oneDoctorsAppointments in allDoctorsAppointments)
        {
        }

        return latestTime;
    }
}