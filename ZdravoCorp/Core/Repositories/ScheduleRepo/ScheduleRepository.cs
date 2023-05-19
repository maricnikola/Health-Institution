using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.AnamnesisReport;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.MedicalRecords;
using ZdravoCorp.Core.Models.Operations;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Counters;

namespace ZdravoCorp.Core.Repositories.ScheduleRepo;

public class ScheduleRepository : ISerializable, IScheduleRepository
{
    private readonly CounterDictionary _counterDictionary;

    private readonly string _fileNameAppointments = @".\..\..\..\Data\appointments.json";
    private string _fileNameOperations = @".\..\..\..\Data\operations.json";

    public ScheduleRepository()
    {
        _appointments = new List<Appointment>();
        _operations = new List<Operation>();
        _counterDictionary = new CounterDictionary();
        Serializer.Load(this);
    }

    private List<Appointment> _appointments { get; set; }
    private List<Operation> _operations { get; }

    public string FileName()
    {
        return _fileNameAppointments;
    }

    public IEnumerable<object>? GetList()
    {
        return _appointments;
    }

    public void Import(JToken token)
    {
        _appointments = token.ToObject<List<Appointment>>();
    }

    public void InsertAppointment(Appointment appointment)
    {
        _appointments.Add(appointment);
        Serializer.Save(this);
    }
    public void InsertOperation(Operation appointment)
    {
        _operations.Add(appointment);
        Serializer.Save(this);
    }
    public void DeleteAppointment(Appointment appointment)
    {
        _appointments.Remove(appointment);
        Serializer.Save(this);
    }

    public void ChangeAppointment(Appointment appointment)
    {
        var index = _appointments.IndexOf(appointment);
        _appointments[index] = appointment;
        Serializer.Save(this);
    }

    public List<Appointment> GetAllAppointments()
    {
        return _appointments;
    }

    public List<Operation> GetAllOperations()
    {
        return _operations;
    }



    public void AddOperation(Operation operation)
    {
        _operations.Add(operation);
        Serializer.Save(this);
    }

    public Operation? GetOperationById(int id)
    {
        return _operations.FirstOrDefault(op => op.Id == id);
    }

    public Appointment GetAppointmentById(int id)
    {
        return _appointments.FirstOrDefault(ap => ap.Id == id);
    }

    public List<Appointment> GetPatientAppointments(string patientMail)
    {
        return _appointments.Where(appointment => appointment.PatientEmail == patientMail && !appointment.IsCanceled)
            .ToList();
    }

    public List<Operation> GetPatientOperations(string patientMail)
    {
        return _operations.Where(operation => operation.MedicalRecord.Patient.Email == patientMail).ToList();
    }

    public List<Appointment> GetPatientsOldAppointments(string patientMail)
    {
        return _appointments.Where(appointment =>
            appointment.PatientEmail == patientMail && appointment.Time.end < DateTime.Now).ToList();
    }

    public List<Appointment> GetDoctorAppointments(string doctorsMail)
    {
        return _appointments.Where(appointment => appointment.Doctor.Email == doctorsMail).ToList();
    }

    public List<Operation> GetDoctorOperations(string doctorsMail)
    {
        return _operations.Where(operation => operation.Doctor.Email == doctorsMail).ToList();
    }

    public bool isDoctorAvailable(TimeSlot timeslot, string doctorsMail)
    {
        var appointments = GetDoctorAppointments(doctorsMail);
        var operations = GetDoctorOperations(doctorsMail);
        return checkAvailability(appointments, operations, timeslot);
    }

    public bool isPatientAvailable(TimeSlot timeslot, string patientMail)
    {
        var appointments = GetPatientAppointments(patientMail);
        var operations = GetPatientOperations(patientMail);
        return checkAvailability(appointments, operations, timeslot);
    }

    public bool checkAvailability(List<Appointment> appointments, List<Operation> operations, TimeSlot timeslot)
    {
        return !appointments.Any(appointment => !appointment.Time.Overlap(timeslot) && !appointment.IsCanceled) && operations.All(operation => operation.Time.Overlap(timeslot) || operation.IsCanceled);
    }

    public Appointment? CreateAppointment(TimeSlot time, Doctor doctor, string email)
    {
        if (!isDoctorAvailable(time, doctor.Email) || !isPatientAvailable(time, email) ||
            time.start <= DateTime.Now) return null;
        var id = IDGenerator.GetId();
        var appointment = new Appointment(id, time, doctor, email);
        _appointments.Add(appointment);
        Serializer.Save(this);
        _counterDictionary.AddNews(appointment.PatientEmail, DateTime.Now);
        return appointment;

    }

    public void CreateOperation(TimeSlot time, Doctor doctor, MedicalRecord medicalRecord)
    {
        if (!isDoctorAvailable(time, doctor.Email) || !isPatientAvailable(time, medicalRecord.Patient.Email)) return;
        var operation = new Operation(0, time, doctor, medicalRecord);
        _operations.Add(operation);
        Serializer.Save(this);
    }

    public Appointment? ChangeAppointment(int id, TimeSlot time, Doctor doctor, string email)
    {
        if (time.start <= DateTime.Now) return null;
        var appointment = new Appointment(id, time, doctor, email);
        if (IsAppointmentInList(appointment))
        {
            MessageBox.Show("Nothing is changed", "Error", MessageBoxButton.OK);
            return null;
        }

        var toGo = GetAppointmentById(id);
        _appointments.Remove(GetAppointmentById(id));
        if (isDoctorAvailable(time, doctor.Email) && isPatientAvailable(time, email))
        {
            _appointments.Add(appointment);
            Serializer.Save(this);
            _counterDictionary.AddCancelation(appointment.PatientEmail, DateTime.Now);
            return appointment;
        }
        _appointments.Add(toGo);
        return null;
    }

    public void CancelAppointment(Appointment appointment)
    {
        var isOnTime = appointment.Time.GetTimeBeforeStart(DateTime.Now) > 24;
        if (!IsAppointmentInList(appointment) || !isOnTime) return;
        var index = _appointments.IndexOf(appointment);
        appointment.IsCanceled = true;
        _appointments[index] = appointment;
        _counterDictionary.AddCancelation(appointment.PatientEmail, DateTime.Now);
        Serializer.Save(this);
    }

    public Appointment CancelAppointmentByDoctor(Appointment appointment)
    {
        if (!IsAppointmentInList(appointment)) return null;
        var index = _appointments.IndexOf(appointment);
        appointment.IsCanceled = true;
        _appointments[index] = appointment;
        Serializer.Save(this);
        return appointment;

    }

    public bool IsAppointmentInList(Appointment appointment)
    {
        //return (from t in Appointments where t.Id == appointment.Id where t.Doctor.Email == appointment.Doctor.Email where t.MedicalRecord.Patient.Email == appointment.MedicalRecord.Patient.Email select t).Any(t => t.Time.start == appointment.Time.start && t.Time.end == appointment.Time.end);
        return _appointments.Any(ap =>
            ap.PatientEmail == appointment.PatientEmail && ap.Doctor.Email == appointment.Doctor.Email &&
            ap.Time.start == appointment.Time.start && ap.Time.end == appointment.Time.end);
    }

    public List<Appointment> GetAppointmentsForShow(DateTime date)
    {
        return _appointments.Where(appointment => IsForShow(appointment, date)).ToList();
    }

    public bool IsForShow(Appointment appointment, DateTime date)
    {
        var dateEnd = date.AddDays(3);
        return appointment.Time.start > date && appointment.Time.start < dateEnd;
    }

    private HashSet<TimeSlot> FindOccupiedTimeSlotsForDoctor(string doctorsMail, List<TimeSlot> timeLimitation)
    {
        var operations = GetDoctorOperations(doctorsMail);
        var appointments = GetDoctorAppointments(doctorsMail);
        var doctorsTimeSlots = new HashSet<TimeSlot>();
        foreach (var operation in operations.Where(operation => operation.Time.IsInsideListOfSlots(timeLimitation)))
            doctorsTimeSlots.Add(operation.Time);
        foreach (var appointment in appointments.Where(appointment => appointment.Time.IsInsideListOfSlots(timeLimitation)))
            doctorsTimeSlots.Add(appointment.Time);
        return doctorsTimeSlots;
    }

    private TimeSlot? FindFirstEmptyTimeSlotForDoctor(HashSet<TimeSlot> doctorsTimeSlots, List<TimeSlot> allDays,
        string doctorsMail)
    {
        foreach (var day in allDays)
        {
            if (day.start < DateTime.Now)
            {
                if (day.end < DateTime.Now)
                    continue;
                day.start = TimeSlot.GiveFirstDevisibleBy15(DateTime.Now);
            }

            while (day.start != day.end)
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

    public TimeSlot? FindAvailableTimeslotsForOneDoctor(string doctorsMail, TimeSlot wantedTime, DateTime lastDate,
        List<TimeSlot>? alreadyUsed = null)
    {
        if (alreadyUsed == null) alreadyUsed = new List<TimeSlot>();
        var wantedTimeCopy = new TimeSlot(wantedTime.start, wantedTime.end);
        var allDays = wantedTimeCopy.GiveSameTimeUntileSomeDay(lastDate);
        var doctorsTimeSlots = FindOccupiedTimeSlotsForDoctor(doctorsMail, allDays);
        foreach (var used in alreadyUsed) doctorsTimeSlots.Add(used);
        var availableTimeSlot = FindFirstEmptyTimeSlotForDoctor(doctorsTimeSlots, allDays, doctorsMail);
        return availableTimeSlot;
    }

    public List<Appointment> FindAppointmentsByDoctorPriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        string patientMail)
    {
        var availableTimeSlots = FindAvailableTimeSlotsByDoctorPriority(doctor.Email, wantedTime, lastDate);
        return availableTimeSlots.Select(slot => new Appointment(IDGenerator.GetId(), slot, doctor, patientMail))
            .ToList();
    }

    public List<Appointment> FindAppointmentsByTimePriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        string patientMail, DoctorRepository doctorRepository)
    {
        var pairsTimeSlotDoctor = FindAvailableTimeSlotsByTimePriority(doctor, wantedTime, lastDate, doctorRepository);
        return pairsTimeSlotDoctor
            .Select(pair => new Appointment(IDGenerator.GetId(), pair.Item1, pair.Item2, patientMail)).ToList();
    }

    private List<TimeSlot> FindAvailableTimeSlotsByDoctorPriority(string doctorMail, TimeSlot wantedTime,
        DateTime lastDate)
    {
        var availableTimeSlots = new List<TimeSlot>();
        var availableTimeSlot = FindAvailableTimeslotsForOneDoctor(doctorMail, wantedTime, lastDate);
        if (availableTimeSlot == null)
            availableTimeSlots = GetNearestSlotsByDoctorPriority(3, doctorMail, wantedTime, lastDate);
        else
            availableTimeSlots.Add(availableTimeSlot);

        return availableTimeSlots;
    }

    private List<TimeSlot> GetNearestSlotsByDoctorPriority(int howMany, string doctorsMail, TimeSlot wantedTime,
        DateTime lastDate)
    {
        var extension = new TimeSpan(2, 0, 0);
        wantedTime = wantedTime.ExtendButStayOnSameDay(extension);
        var nearestThreeSlots = new List<TimeSlot>();
        while (nearestThreeSlots.Count != howMany)
        {
            var availableTimeSlot =
                FindAvailableTimeslotsForOneDoctor(doctorsMail, wantedTime, lastDate, nearestThreeSlots);
            if (availableTimeSlot == null)
            {
                lastDate = lastDate.AddDays(1);
                continue;
            }

            nearestThreeSlots.Add(availableTimeSlot);
        }

        return nearestThreeSlots;
    }

    private List<Tuple<TimeSlot, Doctor>> FindAvailableTimeSlotsByTimePriority(Doctor doctor, TimeSlot wantedTime,
        DateTime lastDate, DoctorRepository doctorRepository)
    {
        var availablePairs = new List<Tuple<TimeSlot, Doctor>>();
        var availableTimeSlot = FindAvailableTimeslotsForOneDoctor(doctor.Email, wantedTime, lastDate);
        if (availableTimeSlot == null)
            availablePairs = GetNearesThreeSlotsByTimePriority(doctor, wantedTime, lastDate, doctorRepository);
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
                var availableTimeSlot =
                    FindAvailableTimeslotsForOneDoctor(sameSpecDoctor.Email, wantedTime, lastDate, finedSlots);
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
            var finedSlots = new List<TimeSlot>();
            for (var i = 0; i < 3; i++)
            {
                var availableTimeSlot =
                    FindAvailableTimeslotsForOneDoctor(doctor.Email, wantedTime, lastDate, finedSlots);
                if (availableTimeSlot == null)
                    break;
                finedSlots.Add(availableTimeSlot);
                nearestThreeSlots.Add(new Tuple<TimeSlot, Doctor>(availableTimeSlot, anyDoctor));
                if (nearestThreeSlots.Count == 3)
                    return nearestThreeSlots;
            }
        }

        var howManyLeftToFind = 3 - nearestThreeSlots.Count;
        var slotsLeftToFind = GetNearestSlotsByDoctorPriority(howManyLeftToFind, doctor.Email, wantedTime, lastDate);
        nearestThreeSlots.AddRange(slotsLeftToFind.Select(slot => new Tuple<TimeSlot, Doctor>(slot, doctor)));

        return nearestThreeSlots;
    }


    public bool IsPatientExamined(Patient patient, Doctor doctor)
    {
        return (from appointment in _appointments let matchingDoctorAndPatient = appointment.PatientEmail.Equals(patient.Email) && appointment.Doctor.Equals(doctor) where matchingDoctorAndPatient && appointment.Status.Equals(true) select appointment).Any();
    }

    public bool CanPerformAppointment(int id)
    {
        var appointment = GetAppointmentById(id);
        return !appointment.IsCanceled && appointment.Time.IsNow();
    }

    public bool CheckPerformingAppointmentData(List<string> symptoms, string opinion, List<string> allergens,
        string keyWord)
    {
        if (checkListElementsLength(symptoms)) return false;
        if (opinion.Trim().Length < 10) return false;
        if (checkListElementsLength(allergens)) return false;
        return keyWord.Trim().Length >= 2;
    }

    private bool checkListElementsLength(List<string> list)
    {
        return list.Any(l => l.Trim().Length < 5);
    }

    public void ChangePerformingAppointment(int id, List<string> symptoms, string opinion, List<string> allergens,
        string keyWord, int roomId)
    {
        var appointment = GetAppointmentById(id);
        _appointments.Remove(GetAppointmentById(id));
        var anamnesis = new Anamnesis(symptoms, opinion, keyWord, allergens);
        var performedAppointment = new Appointment(appointment.Id, appointment.Time, appointment.Doctor,
            appointment.PatientEmail, anamnesis);
        performedAppointment.Status = true;
        performedAppointment.Room = roomId;
        _appointments.Add(performedAppointment);
        Serializer.Save(this);
    }

    public Appointment GetPatientsFirstAppointment(string patientEmail, TimeSlot interval)
    {
        //Appointment appointment = null; 
        return _appointments.FirstOrDefault(appointment => appointment.PatientEmail.Equals(patientEmail) && appointment.Time.IsInsideSingleSlot(interval));
    }
}