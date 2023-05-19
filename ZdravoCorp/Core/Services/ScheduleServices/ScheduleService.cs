using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.Models.AnamnesisReport;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.MedicalRecords;
using ZdravoCorp.Core.Models.Operations;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Counters;

namespace ZdravoCorp.Core.Services.ScheduleServices;

public class ScheduleService : IScheduleService
{
    private IScheduleRepository _scheduleRepository;

    public ScheduleService(IScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }

    public void AddAppointment(AppointmentDTO appointmentDTO)
    {
        _scheduleRepository.InsertAppointment(new Appointment(appointmentDTO));
    }

    public void AddOperation(OperationDTO operationDTO)
    {
        _scheduleRepository.InsertOperation(new Operation(operationDTO));
    }

    public Operation? GetOperationById(int id)
    {
        return _scheduleRepository.GetAllOperations().FirstOrDefault(op => op.Id == id);
    }

    public Appointment? GetAppointmentById(int id)
    {
        return _scheduleRepository.GetAllAppointments().FirstOrDefault(op => op.Id == id);
    }

    public List<Appointment> GetAllAppointments()
    {
        return _scheduleRepository.GetAllAppointments();
    }

    public List<Appointment> GetPatientAppointments(string patientMail)
    {
        return _scheduleRepository.GetAllAppointments().Where(appointment => appointment.PatientEmail == patientMail && !appointment.IsCanceled)
            .ToList();
    }

    public List<Operation> GetPatientOperations(string patientMail)
    {
        return _scheduleRepository.GetAllOperations().Where(operation => operation.MedicalRecord.Patient.Email == patientMail).ToList();
    }

    public List<Appointment> GetPatientsOldAppointments(string patientMail)
    {
        return _scheduleRepository.GetAllAppointments().Where(appointment =>
            appointment.PatientEmail == patientMail && appointment.Time.end < DateTime.Now).ToList();
    }
    public List<Appointment> GetDoctorAppointments(string doctorsMail)
    {
        return _scheduleRepository.GetAllAppointments().Where(appointment => appointment.Doctor.Email == doctorsMail).ToList();
    }

    public List<Operation> GetDoctorOperations(string doctorsMail)
    {
        return _scheduleRepository.GetAllOperations().Where(operation => operation.Doctor.Email == doctorsMail).ToList();
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
        _scheduleRepository.InsertAppointment(appointment);
        return appointment;
    }

    public void CreateOperation(TimeSlot time, Doctor doctor, MedicalRecord medicalRecord)
    {
        if (!isDoctorAvailable(time, doctor.Email) || !isPatientAvailable(time, medicalRecord.Patient.Email)) return;
        var operation = new Operation(0, time, doctor, medicalRecord);
        _scheduleRepository.InsertOperation(operation);
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
        _scheduleRepository.DeleteAppointment(GetAppointmentById(id));
        if (isDoctorAvailable(time, doctor.Email) && isPatientAvailable(time, email))
        {
            _scheduleRepository.InsertAppointment(appointment);
            //  _counterDictionary.AddCancelation(appointment.PatientEmail, DateTime.Now);
            return appointment;
        }
        _scheduleRepository.InsertAppointment(appointment);
        return null;
    }

    public void CancelAppointment(AppointmentDTO appointmentDTO)
    {
        Appointment appointment = new Appointment(appointmentDTO);
        var isOnTime = appointment.Time.GetTimeBeforeStart(DateTime.Now) > 24;
        if (!IsAppointmentInList(appointment) || !isOnTime) return;
        _scheduleRepository.DeleteAppointment(GetAppointmentById(appointment.Id));
        appointment.IsCanceled = true;
        _scheduleRepository.InsertAppointment(appointment);
        //_counterDictionary.AddCancelation(appointment.PatientEmail, DateTime.Now);
    }

    public Appointment CancelAppointmentByDoctor(AppointmentDTO appointmentDTO)
    {
        Appointment appointment = new Appointment(appointmentDTO);
        if (!IsAppointmentInList(appointment)) return null;
        _scheduleRepository.DeleteAppointment(GetAppointmentById(appointment.Id));
        appointment.IsCanceled = true;
        _scheduleRepository.InsertAppointment(appointment);
        return appointment;
    }

    public bool IsAppointmentInList(Appointment appointment)
    {
        return _scheduleRepository.GetAllAppointments().Any(ap =>
            ap.PatientEmail == appointment.PatientEmail && ap.Doctor.Email == appointment.Doctor.Email &&
            ap.Time.start == appointment.Time.start && ap.Time.end == appointment.Time.end);
    }

    public List<Appointment> GetAppointmentsForShow(DateTime date)
    {
        return _scheduleRepository.GetAllAppointments().Where(appointment => IsForShow(appointment, date)).ToList();
    }

    public bool IsForShow(Appointment appointment, DateTime date)
    {
        var dateEnd = date.AddDays(3);
        return appointment.Time.start > date && appointment.Time.start < dateEnd;
    }

    public HashSet<TimeSlot> FindOccupiedTimeSlotsForDoctor(string doctorsMail, List<TimeSlot> timeLimitation)
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

    public TimeSlot? FindFirstEmptyTimeSlotForDoctor(HashSet<TimeSlot> doctorsTimeSlots, List<TimeSlot> allDays, string doctorsMail)
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

    public List<Appointment> FindAppointmentsByDoctorPriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate, string patientMail)
    {
        var availableTimeSlots = FindAvailableTimeSlotsByDoctorPriority(doctor.Email, wantedTime, lastDate);
        return availableTimeSlots.Select(slot => new Appointment(IDGenerator.GetId(), slot, doctor, patientMail))
            .ToList();
    }

    public List<Appointment> FindAppointmentsByTimePriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate, string patientMail,
        DoctorRepository doctorRepository)
    {
        var pairsTimeSlotDoctor = FindAvailableTimeSlotsByTimePriority(doctor, wantedTime, lastDate, doctorRepository);
        return pairsTimeSlotDoctor
            .Select(pair => new Appointment(IDGenerator.GetId(), pair.Item1, pair.Item2, patientMail)).ToList();
    }

    public List<TimeSlot> FindAvailableTimeSlotsByDoctorPriority(string doctorMail, TimeSlot wantedTime,
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

    public List<TimeSlot> GetNearestSlotsByDoctorPriority(int howMany, string doctorsMail, TimeSlot wantedTime, DateTime lastDate)
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

    public List<Tuple<TimeSlot, Doctor>> FindAvailableTimeSlotsByTimePriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        DoctorRepository doctorRepository)
    {
        var availablePairs = new List<Tuple<TimeSlot, Doctor>>();
        var availableTimeSlot = FindAvailableTimeslotsForOneDoctor(doctor.Email, wantedTime, lastDate);
        if (availableTimeSlot == null)
            availablePairs = GetNearesThreeSlotsByTimePriority(doctor, wantedTime, lastDate, doctorRepository);
        else
            availablePairs.Add(new Tuple<TimeSlot, Doctor>(availableTimeSlot, doctor));

        return availablePairs;
    }

    public List<Tuple<TimeSlot, Doctor>> GetNearesThreeSlotsByTimePriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        DoctorRepository doctorRepository)
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
        return (from appointment in _scheduleRepository.GetAllAppointments() let matchingDoctorAndPatient = appointment.PatientEmail.Equals(patient.Email) && appointment.Doctor.Equals(doctor) where matchingDoctorAndPatient && appointment.Status.Equals(true) select appointment).Any();
    }

    public bool CanPerformAppointment(int id)
    {
        var appointment = GetAppointmentById(id);
        return !appointment.IsCanceled && appointment.Time.IsNow();
    }

    public bool CheckPerformingAppointmentData(List<string> symptoms, string opinion, List<string> allergens, string keyWord)
    {
        if (checkListElementsLength(symptoms)) return false;
        if (opinion.Trim().Length < 10) return false;
        if (checkListElementsLength(allergens)) return false;
        return keyWord.Trim().Length >= 2;
    }

    public void ChangePerformingAppointment(int id, List<string> symptoms, string opinion, List<string> allergens, string keyWord, int roomId)
    {
        var appointment = GetAppointmentById(id);
        _scheduleRepository.DeleteAppointment(appointment);
        var anamnesis = new Anamnesis(symptoms, opinion, keyWord, allergens);
        var performedAppointment = new Appointment(appointment.Id, appointment.Time, appointment.Doctor,
            appointment.PatientEmail, anamnesis);
        performedAppointment.Status = true;
        performedAppointment.Room = roomId;
        _scheduleRepository.InsertAppointment(performedAppointment);
    }

    public bool checkListElementsLength(List<string> list)
    {
        return list.Any(l => l.Trim().Length < 5);
    }

    public Appointment GetPatientsFirstAppointment(string patientEmail, TimeSlot interval)
    {
        return _scheduleRepository.GetAllAppointments().FirstOrDefault(appointment => appointment.PatientEmail.Equals(patientEmail) && appointment.Time.IsInsideSingleSlot(interval));

    }
}