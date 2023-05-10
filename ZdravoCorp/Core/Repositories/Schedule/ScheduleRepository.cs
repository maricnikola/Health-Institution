using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Windows;
using ZdravoCorp.Core.Counters;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.Operation;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.Repositories.Schedule;

public class ScheduleRepository
{
    private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {

    };

    private String _fileNameAppointments = @".\..\..\..\Data\appointments.json";
    private String _fileNameOperations = @".\..\..\..\Data\operations.json";
    private CounterDictionary _counterDictionary;
    private List<Appointment> Appointments { get; set; }
    private List<Operation> Operations { get; set; }

    public ScheduleRepository()
    {
        Appointments = new List<Appointment>();
        Operations = new List<Operation>();
        _counterDictionary = new CounterDictionary();
    }

    //public ScheduleRepository()
    //{
    //    LoadAppointments();
    //    Operations = new List<Operation>();
    //}

    public void AddAppointment(Appointment appointment)
    {
        Appointments.Add(appointment);
    }

    public void AddOperation(Operation operation)
    {
        Operations.Add(operation);
    }

    public Operation? GetOperationById(int id)
    {
        return Operations.FirstOrDefault(op => op.Id == id);
    }

    public Appointment GetAppointmentById(int id)
    {
        return Appointments.FirstOrDefault(ap => ap.Id == id);
    }

    public List<Appointment> GetAllAppointments()
    {
        return Appointments;
    }

    public List<Appointment> GetPatientAppointments(String patientMail)
    {
        List<Appointment> patientAppointments = new List<Appointment>();   
        foreach(Appointment appointment in Appointments)
        {
            if(appointment.MedicalRecord.user.Email == patientMail && !appointment.IsCanceled) patientAppointments.Add(appointment);
        }
        return patientAppointments;
    }
    public List<Operation> GetPatientOperations(String patientMail)
    {
        List<Operation> patientOperations = new List<Operation>();
        foreach(Operation operation in Operations)
        {
            if (operation.MedicalRecord.user.Email == patientMail) patientOperations.Add(operation);
        }
        return patientOperations;
    }
   
    public List<Appointment> GetDoctorAppointments(String doctorsMail)
    {
        List<Appointment> doctorAppointments = new List<Appointment>();
        foreach(Appointment appointment in Appointments)
        {
            if(appointment.Doctor.Email == doctorsMail) doctorAppointments.Add(appointment);
        }
        return doctorAppointments;
    }

    public List<Operation> GetDoctorOperations(String doctorsMail)
    {
        List<Operation> doctorOperations = new List<Operation>();
        foreach (Operation operation in Operations)
        {
            if (operation.Doctor.Email == doctorsMail) doctorOperations.Add(operation);
        }
        return doctorOperations;
    }

    public bool isDoctorAvailable(TimeSlot timeslot, string doctorsMail)
    {
        List<Appointment> appointments = GetDoctorAppointments(doctorsMail);
        List<Operation> operations = GetDoctorOperations(doctorsMail);
        return checkAvailability(appointments, operations, timeslot);
    }

    public bool isPatientAvailable(TimeSlot timeslot, String patientMail)
    {
        List<Appointment> appointments = GetPatientAppointments(patientMail);
        List<Operation> operations = GetPatientOperations(patientMail);
        return checkAvailability(appointments, operations, timeslot);
    }

    public bool checkAvailability(List<Appointment> appointments, List<Operation> operations, TimeSlot timeslot)
    {
        foreach (var appointment in appointments)
        {
            if (!appointment.Time.Overlap(timeslot) && !appointment.IsCanceled)
                return false;
        }
        foreach (var operation in operations)
        {
            if (!operation.Time.Overlap(timeslot) && !operation.IsCanceled)
                return false;
        }
        return true;
    }

    public void LoadAppointments()
    {
        string text = File.ReadAllText(_fileNameAppointments);
        var appointments = JsonSerializer.Deserialize<List<Appointment>>(text);
        appointments.ForEach(appointment => Appointments.Add(appointment));

    }

    public void SaveAppointments()
    {
        var users = JsonSerializer.Serialize(this.Appointments, _serializerOptions);
        File.WriteAllText(this._fileNameAppointments, users);
    }

    public void LoadOperations()
    {
        string text = File.ReadAllText(_fileNameOperations);
        var operatons = JsonSerializer.Deserialize<List<Operation>>(text);
        Operations.ForEach(operations => Operations.Add(operations));

    }

    public void SaveOperations()
    {
        var users = JsonSerializer.Serialize(this.Operations, _serializerOptions);
        File.WriteAllText(this._fileNameOperations, users);
    }

    public Appointment? CreateAppointment(TimeSlot time, Doctor doctor, Models.MedicalRecord.MedicalRecord medicalRecord)
    {
        if (isDoctorAvailable(time,doctor.Email) && isPatientAvailable(time, medicalRecord.user.Email) && time.start>DateTime.Now)
        {
            Random random = new Random();
            int id = random.Next(0, 100000);
            Appointment appointment = new Appointment(id, time, doctor, medicalRecord);
            Appointments.Add(appointment);
            SaveAppointments();
            _counterDictionary.AddNews(appointment.MedicalRecord.user.Email, DateTime.Now);
            return appointment;
        }

        return null;
    }
    public void CreateOperation(TimeSlot time, Doctor doctor, Models.MedicalRecord.MedicalRecord medicalRecord)
    {
        if (isDoctorAvailable(time, doctor.Email) && isPatientAvailable(time, medicalRecord.user.Email))
        {
            Operation operation = new Operation(0, time, doctor, medicalRecord);
            Operations.Add(operation);
            SaveOperations();
        }
    }

    public Appointment? ChangeAppointment(int id,TimeSlot time, Doctor doctor, Models.MedicalRecord.MedicalRecord medicalRecord)
    {
        if (time.start > DateTime.Now)
        {
            Appointment appointment = new Appointment(id, time, doctor, medicalRecord);
            if (IsAppointmentInList(appointment))
            {
                MessageBox.Show("Nothing is changed", "Error", MessageBoxButton.OK);
                return null;
            }
            else
            {
                Appointment toGo = GetAppointmentById(id);
                Appointments.Remove(GetAppointmentById(id));
                if (isDoctorAvailable(time, doctor.Email) && isPatientAvailable(time, medicalRecord.user.Email))
                {
                    Appointments.Add(appointment);
                    SaveAppointments();
                    _counterDictionary.AddCancelation(appointment.MedicalRecord.user.Email, DateTime.Now);
                    return appointment;
                }
                Appointments.Add(toGo);
                return null;
            }

        }
        return null;
    }

    public void CancelAppointment(Appointment appointment)
    {
        bool isOnTime = appointment.Time.GetTimeBeforeStart(DateTime.Now)>24;
        if (IsAppointmentInList(appointment) && isOnTime)
        {
            int index = Appointments.IndexOf(appointment);
            appointment.IsCanceled = true;
            Appointments[index] = appointment;
            _counterDictionary.AddCancelation(appointment.MedicalRecord.user.Email, DateTime.Now);
            SaveAppointments();
        }
    }

    public bool IsAppointmentInList(Appointment appointment)
    {
        //return (from t in Appointments where t.Id == appointment.Id where t.Doctor.Email == appointment.Doctor.Email where t.MedicalRecord.user.Email == appointment.MedicalRecord.user.Email select t).Any(t => t.Time.start == appointment.Time.start && t.Time.end == appointment.Time.end);
        return Appointments.Any(ap => ap.MedicalRecord.user.Email == appointment.MedicalRecord.user.Email && ap.Doctor.Email == appointment.Doctor.Email && ap.Time.start==appointment.Time.start && ap.Time.end==appointment.Time.end);
    }

    public List<Appointment> GetAppointmentsForShow(DateTime date)
    {
        List<Appointment> showAppointments = new List<Appointment>();
        foreach(Appointment appointment in Appointments)
        {
            if (IsForShow(appointment, date)) showAppointments.Add(appointment);
        }
        return showAppointments;
    }

    public bool IsForShow(Appointment appointment,DateTime date)
    {
        DateTime dateEnd = date.AddDays(3);
        return (appointment.Time.start > date && appointment.Time.start < dateEnd);
    }

    private HashSet<TimeSlot> FindOccupiedTimeSlotsForDoctor(String doctorsMail, List<TimeSlot> timeLimitation)
    {
        List<Operation> operations = GetDoctorOperations(doctorsMail);
        List<Appointment> appointments = GetDoctorAppointments(doctorsMail);
        HashSet<TimeSlot> doctorsTimeSlots = new HashSet<TimeSlot>();
        foreach (var operation in operations)
        {
            if (operation.Time.IsInsideListOfSlots(timeLimitation))
                doctorsTimeSlots.Add(operation.Time);
        }
        foreach (var appointment in appointments)
        {
            if (appointment.Time.IsInsideListOfSlots(timeLimitation))
                doctorsTimeSlots.Add(appointment.Time);
        } 
        return doctorsTimeSlots;
    }

    private TimeSlot? FindFirstEmptyTimeSlotForDoctor(HashSet<TimeSlot> doctorsTimeSlots, List<TimeSlot> allDays, string doctorsMail)
    {
        foreach (var day in allDays)
        {
            if (day.start < DateTime.Now)
            {
                if (day.end<DateTime.Now)
                    continue;
                day.start = GiveFirstDevisibleBy15(DateTime.Now);
            }
            while (day.start!=day.end)
            {
                var slotForAppointment = new TimeSlot(day.start, day.start.AddMinutes(15));
                if (!doctorsTimeSlots.Contains(slotForAppointment))
                {
                    if (isDoctorAvailable(slotForAppointment, doctorsMail)) return slotForAppointment;
                    day.start = day.start.AddMinutes(15);
                    continue;
                }
                day.start = day.start.AddMinutes(15);
            }
        }

        return null;
    }

    private TimeSlot? FindAvailableTimeslotsForOneDoctor(string doctorsMail, TimeSlot wantedTime, DateTime lastDate, List<TimeSlot>? alreadyUsed = null)
    {
        if (alreadyUsed==null) alreadyUsed = new List<TimeSlot>();
        var wantedTimeCopy = new TimeSlot(wantedTime.start, wantedTime.end);
        List<TimeSlot> allDays = wantedTimeCopy.GiveSameTimeUntileSomeDay(lastDate);
        HashSet<TimeSlot> doctorsTimeSlots = FindOccupiedTimeSlotsForDoctor(doctorsMail, allDays);
        foreach (var used in alreadyUsed)
        {
            doctorsTimeSlots.Add(used);
        }
        TimeSlot? availableTimeSlot = FindFirstEmptyTimeSlotForDoctor(doctorsTimeSlots, allDays, doctorsMail);
        return availableTimeSlot;
    }

    public List<Appointment> FindAppointmentsByDoctorPriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate, Models.MedicalRecord.MedicalRecord patient)
    {
        var availableTimeSlots = FindAvailableTimeSlotsByDoctorPriority(doctor.Email, wantedTime, lastDate);
        Random random = new Random();
        return availableTimeSlots.Select(slot => new Appointment(random.Next(10000), slot, doctor, patient)).ToList();
    }

    public List<Appointment> FindAppointmentsByTimePriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        Models.MedicalRecord.MedicalRecord patient, DoctorRepository doctorRepository)
    {
        var pairsTimeSlotDoctor = FindAvailableTimeSlotsByTimePriority(doctor, wantedTime, lastDate, doctorRepository);
        Random random = new Random();
        return pairsTimeSlotDoctor.Select(pair => new Appointment(random.Next(10000), pair.Item1, pair.Item2, patient)).ToList();
    }

    private List<TimeSlot> FindAvailableTimeSlotsByDoctorPriority(string doctorMail, TimeSlot wantedTime, DateTime lastDate)
    {
        var availableTimeSlots = new List<TimeSlot>();
        TimeSlot? availableTimeSlot = FindAvailableTimeslotsForOneDoctor(doctorMail, wantedTime, lastDate);
        if (availableTimeSlot == null)
        {
            availableTimeSlots = GetNearestSlotsByDoctorPriority(3, doctorMail, wantedTime, lastDate);
        }
        else
            availableTimeSlots.Add(availableTimeSlot);

        return availableTimeSlots;
    }

    private List<TimeSlot> GetNearestSlotsByDoctorPriority(int howMany, string doctorsMail, TimeSlot wantedTime, DateTime lastDate)
    {
        var extension = new TimeSpan(2, 0, 0);
        wantedTime = wantedTime.ExtendButStayOnSameDay(extension);
        var nearestThreeSlots = new List<TimeSlot>();
        while (nearestThreeSlots.Count != howMany)
        {
            var availableTimeSlot = FindAvailableTimeslotsForOneDoctor(doctorsMail, wantedTime, lastDate, nearestThreeSlots);
            if (availableTimeSlot == null)
            {
                lastDate = lastDate.AddDays(1);
                continue;
            }
            nearestThreeSlots.Add(availableTimeSlot);
        }
        return nearestThreeSlots;
    }

    private List<Tuple<TimeSlot, Doctor>> FindAvailableTimeSlotsByTimePriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate, DoctorRepository doctorRepository)
    {
        var availablePairs = new List<Tuple<TimeSlot, Doctor>>();
        TimeSlot? availableTimeSlot = FindAvailableTimeslotsForOneDoctor(doctor.Email, wantedTime, lastDate);
        if (availableTimeSlot == null)
        {
            availablePairs = GetNearesThreeSlotsByTimePriority(doctor, wantedTime, lastDate, doctorRepository);
        }
        else
            availablePairs.Add(new Tuple<TimeSlot, Doctor>(availableTimeSlot, doctor));


        return availablePairs;
    }

    private List<Tuple<TimeSlot, Doctor>> GetNearesThreeSlotsByTimePriority(Doctor doctor,
        TimeSlot wantedTime, DateTime lastDate, DoctorRepository doctorRepository)
    {
        var nearestThreeSlots = new List<Tuple<TimeSlot, Doctor>>();

        foreach (var sameSpecDoctor in doctorRepository.GetAllWithCertainSpecialization(doctor.Specialization))
        {
            var finedSlots = new List<TimeSlot>();
            for (var i = 0; i < 3; i++)
            {
                var availableTimeSlot = FindAvailableTimeslotsForOneDoctor(sameSpecDoctor.Email, wantedTime, lastDate, finedSlots);
                if (availableTimeSlot == null)
                    break;
                finedSlots.Add(availableTimeSlot);
                nearestThreeSlots.Add(new Tuple<TimeSlot, Doctor>(availableTimeSlot, sameSpecDoctor));
                if (nearestThreeSlots.Count == 3)
                    return nearestThreeSlots;
            }
        }

        foreach (var anyDoctor in doctorRepository.GetAll())
        {
            if (anyDoctor.Specialization == doctor.Specialization) continue;
            List<TimeSlot> finedSlots = new List<TimeSlot>();
            for (var i = 0; i < 3; i++)
            {
                var availableTimeSlot = FindAvailableTimeslotsForOneDoctor(doctor.Email, wantedTime, lastDate, finedSlots);
                if (availableTimeSlot == null)
                    break;
                finedSlots.Add(availableTimeSlot);
                nearestThreeSlots.Add(new Tuple<TimeSlot, Doctor>(availableTimeSlot, anyDoctor));
                if (nearestThreeSlots.Count == 3)
                    return nearestThreeSlots;
            }
        }

        int howManyLeftToFind = 3 - nearestThreeSlots.Count;
        var slotsLeftToFind = GetNearestSlotsByDoctorPriority(howManyLeftToFind, doctor.Email, wantedTime, lastDate);
        nearestThreeSlots.AddRange(slotsLeftToFind.Select(slot => new Tuple<TimeSlot, Doctor>(slot, doctor)));

        return nearestThreeSlots;
    }



    private DateTime GiveFirstDevisibleBy15(DateTime time)       //this should be somewhere else
    {
        var minutes = time.Minute;
        var minutesToAdd = minutes switch
        {
            < 15 => 15 - minutes,
            < 30 => 30 - minutes,
            < 45 => 45 - minutes,
            < 60 => 60 - minutes,
            _ => 0
        };
        return time.AddMinutes(minutesToAdd);
    }

}