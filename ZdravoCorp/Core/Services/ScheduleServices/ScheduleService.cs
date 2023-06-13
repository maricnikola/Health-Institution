using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using ZdravoCorp.Core.Models.AnamnesisReport;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.MedicalRecords;
using ZdravoCorp.Core.Models.Operations;
using ZdravoCorp.Core.Models.Presriptions;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.UserServices;
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
            appointment.PatientEmail == patientMail && appointment.Time.End < DateTime.Now).ToList();
    }
    public List<Appointment> GetDoctorAppointments(string doctorsMail)
    {
        return _scheduleRepository.GetAllAppointments().Where(appointment => appointment.Doctor.Email == doctorsMail).ToList();
    }

    public List<Operation> GetDoctorOperations(string doctorsMail)
    {
        return _scheduleRepository.GetAllOperations().Where(operation => operation.Doctor.Email == doctorsMail).ToList();
    }

    public bool IsDoctorAvailable(TimeSlot timeslot, string doctorsMail)
    {
        var appointments = GetDoctorAppointments(doctorsMail);
        var operations = GetDoctorOperations(doctorsMail);
        return CheckAvailability(appointments, operations, timeslot);
    }

    public bool IsPatientAvailable(TimeSlot timeslot, string patientMail)
    {
        var appointments = GetPatientAppointments(patientMail);
        var operations = GetPatientOperations(patientMail);
        return CheckAvailability(appointments, operations, timeslot);
    }

    public bool CheckAvailability(List<Appointment> appointments, List<Operation> operations, TimeSlot timeslot)
    {
        return !appointments.Any(appointment => appointment.Time.Overlap(timeslot) && !appointment.IsCanceled) && operations.All(operation => operation.Time.Overlap(timeslot) || operation.IsCanceled);
    }

    public Appointment? CreateAppointment(TimeSlot time, Doctor doctor, string email,int roomId)
    {
        if (!IsDoctorAvailable(time, doctor.Email) || !IsPatientAvailable(time, email) ||
            time.Start <= DateTime.Now) return null;
        var id = IDGenerator.GetId();
        var appointment = new Appointment(id, time, doctor, email,roomId);
        _scheduleRepository.InsertAppointment(appointment);
        return appointment;
    }

    public void CreateOperation(TimeSlot time, Doctor doctor, MedicalRecord medicalRecord)
    {
        if (!IsDoctorAvailable(time, doctor.Email) || !IsPatientAvailable(time, medicalRecord.Patient.Email)) return;
        var operation = new Operation(0, time, doctor, medicalRecord);
        _scheduleRepository.InsertOperation(operation);
    }
    public Appointment? ChangeAppointment(int id, TimeSlot time, Doctor doctor, string email)
    {
        if (time.Start <= DateTime.Now) return null;
        var appointment = new Appointment(id, time, doctor, email);
        if (IsAppointmentInList(appointment))
        {
            MessageBox.Show("Nothing is changed", "Error", MessageBoxButton.OK);
            return null;
        }

        var toGo = GetAppointmentById(id);
        _scheduleRepository.DeleteAppointment(GetAppointmentById(id));
        if (IsDoctorAvailable(time, doctor.Email) && IsPatientAvailable(time, email))
        {
            _scheduleRepository.InsertAppointment(appointment);
            //  _counterDictionary.AddCancelation(appointment.PatientEmail, DateTime.Now);
            return appointment;
        }
        _scheduleRepository.InsertAppointment(appointment);
        return null;
    }

    public void CancelAppointment(int id)
    {
        var appointment = GetAppointmentById(id);
        var isOnTime = appointment.Time.GetTimeBeforeStart(DateTime.Now) > 24;
        if (!IsAppointmentInList(appointment) || !isOnTime) return;
        _scheduleRepository.DeleteAppointment(GetAppointmentById(appointment.Id));
        appointment.IsCanceled = true;
        _scheduleRepository.InsertAppointment(appointment);
        //_counterDictionary.AddCancelation(appointment.PatientEmail, DateTime.Now);
    }

    public Appointment CancelAppointmentByDoctor(AppointmentDTO appointmentDTO)
    {
        var appointment = GetAppointmentById(appointmentDTO.Id);
        //Appointment appointment = new Appointment(appointmentDTO);
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
            ap.Time.Start == appointment.Time.Start && ap.Time.End == appointment.Time.End && ap.Status== appointment.Status
            && ap.IsCanceled == appointment.IsCanceled);
    }

    public List<Appointment> GetAppointmentsForShow(DateTime date)
    {
        return _scheduleRepository.GetAllAppointments().Where(appointment => IsForShow(appointment, date)).ToList();
    }

    public bool IsForShow(Appointment appointment, DateTime date)
    {
        var dateEnd = date.AddDays(3);
        return appointment.Time.Start > date && appointment.Time.Start < dateEnd;
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

    public TimeSlot? FindFirstEmptyTimeSlotForDoctor(HashSet<TimeSlot> doctorsTimeSlots, List<TimeSlot> allDays, string doctorsMail,bool isExtended)
    {
        foreach (var day in allDays)
        {
            if (day.Start < DateTime.Now)
            {
                if (day.End < DateTime.Now)
                    continue;
                day.Start = TimeSlot.GiveFirstDevisibleBy15(DateTime.Now);
            }
            if (isExtended)
            {
                if (GetSlotForAppointmentWhenExtended(doctorsTimeSlots, doctorsMail, day, out var slotForAppointment)) return slotForAppointment;
            }
            while (day.Start != day.End)
            {
                var slotForAppointmentNext = new TimeSlot(day.Start, day.Start.AddMinutes(15));
                if (CheckIsSlotAvailable(doctorsTimeSlots, doctorsMail, out var slotForAppointment, slotForAppointmentNext)) return slotForAppointment;
                day.Start = day.Start.AddMinutes(15);
            }
        }
        return null;
    }

    private bool GetSlotForAppointmentWhenExtended(HashSet<TimeSlot> doctorsTimeSlots, string doctorsMail, TimeSlot day,
        out TimeSlot? slotForAppointment)
    {
        var oldEnd = day.End.AddHours(-2);
        var oldStart = oldEnd.AddMinutes(-15);

        while (oldEnd != day.End || oldStart != day.Start)
        {
            var slotForAppointmentBefore = new TimeSlot(oldStart, oldStart.AddMinutes(15));
            var slotForAppointmentAfter = new TimeSlot(oldEnd, oldEnd.AddMinutes(15));

            if (CheckIsSlotAvailable(doctorsTimeSlots, doctorsMail, out slotForAppointment, slotForAppointmentBefore)) return true;

            if (CheckIsSlotAvailable(doctorsTimeSlots, doctorsMail, out slotForAppointment, slotForAppointmentAfter)) return true;

            oldEnd = oldEnd.AddMinutes(15);
            oldStart = oldStart.AddMinutes(-15);
        }

        slotForAppointment = null;
        return false;
    }

    private bool CheckIsSlotAvailable(HashSet<TimeSlot> doctorsTimeSlots, string doctorsMail, out TimeSlot slotForAppointment,
        TimeSlot slotForAppointmentToCheck)
    {
        slotForAppointment = null;
        if (doctorsTimeSlots.Contains(slotForAppointmentToCheck) ||
            !IsDoctorAvailable(slotForAppointmentToCheck, doctorsMail)) return false;
        slotForAppointment = slotForAppointmentToCheck;
        return true;
    }

    public TimeSlot? FindAvailableTimeslotForOneDoctor(string doctorsMail, TimeSlot wantedTime, DateTime lastDate,
        List<TimeSlot>? alreadyUsed = null, bool isExtended=false)
    {
        if (alreadyUsed == null) alreadyUsed = new List<TimeSlot>();
        var wantedTimeCopy = new TimeSlot(wantedTime.Start, wantedTime.End);
        var allDays = wantedTimeCopy.GiveSameTimeUntilSomeDay(lastDate);
        var doctorsTimeSlots = FindOccupiedTimeSlotsForDoctor(doctorsMail, allDays);
        foreach (var used in alreadyUsed) doctorsTimeSlots.Add(used);
        var availableTimeSlot = FindFirstEmptyTimeSlotForDoctor(doctorsTimeSlots, allDays, doctorsMail,isExtended);
        return availableTimeSlot;
    }

    public List<Appointment> FindAppointmentsByDoctorPriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate, string patientMail)
    {
        var availableTimeSlots = FindAvailableTimeSlotsByDoctorPriority(doctor.Email, wantedTime, lastDate);
        return availableTimeSlots.Select(slot => new Appointment(IDGenerator.GetId(), slot, doctor, patientMail))
            .ToList();
    }

    public List<Appointment> FindAppointmentsByTimePriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate, string patientMail,
        IDoctorService doctorService)
    {
        var pairsTimeSlotDoctor = FindAvailableTimeSlotsByTimePriority(doctor, wantedTime, lastDate, doctorService);
        return pairsTimeSlotDoctor
            .Select(pair => new Appointment(IDGenerator.GetId(), pair.Item1, pair.Item2, patientMail)).ToList();
    }

    public List<TimeSlot> FindAvailableTimeSlotsByDoctorPriority(string doctorMail, TimeSlot wantedTime,
        DateTime lastDate)
    {
        var availableTimeSlots = new List<TimeSlot>();
        var availableTimeSlot = FindAvailableTimeslotForOneDoctor(doctorMail, wantedTime, lastDate);
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
                FindAvailableTimeslotForOneDoctor(doctorsMail, wantedTime, lastDate, nearestThreeSlots, true);
            if (availableTimeSlot == null)
            {
                lastDate = lastDate.AddDays(1);
                continue;
            }
            nearestThreeSlots.Add(availableTimeSlot);
            nearestThreeSlots.Sort();
        }
        return nearestThreeSlots;
    }

    public List<Tuple<TimeSlot, Doctor>> FindAvailableTimeSlotsByTimePriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        IDoctorService doctorService)
    {
        var availablePairs = new List<Tuple<TimeSlot, Doctor>>();
        var availableTimeSlot = FindAvailableTimeslotForOneDoctor(doctor.Email, wantedTime, lastDate);
        if (availableTimeSlot == null)
            availablePairs = GetNearesThreeSlotsByTimePriority(doctor, wantedTime, lastDate, doctorService);
        else
            availablePairs.Add(new Tuple<TimeSlot, Doctor>(availableTimeSlot, doctor));

        return availablePairs;
    }
//adadadada
    public List<Tuple<TimeSlot, Doctor>> GetNearesThreeSlotsByTimePriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        IDoctorService doctorService)
    {
        var nearestThreeSlots = new List<Tuple<TimeSlot, Doctor>>();

        foreach (var sameSpecDoctor in doctorService.GetAllWithCertainSpecialization(doctor.Specialization))
        {
            if (FindThreeSlotsForDoctor(wantedTime, lastDate, sameSpecDoctor, nearestThreeSlots, out var tuples)) return tuples;
        }

        foreach (var anyDoctor in doctorService.GetAll().Where(anyDoctor => anyDoctor.Specialization != doctor.Specialization))
        {
            if (FindThreeSlotsForDoctor(wantedTime, lastDate, anyDoctor, nearestThreeSlots, out var tuples)) return tuples;
        }

        var howManyLeftToFind = 3 - nearestThreeSlots.Count;
        var slotsLeftToFind = GetNearestSlotsByDoctorPriority(howManyLeftToFind, doctor.Email, wantedTime, lastDate);
        nearestThreeSlots.AddRange(slotsLeftToFind.Select(slot => new Tuple<TimeSlot, Doctor>(slot, doctor)));

        return nearestThreeSlots;
    }

    private bool FindThreeSlotsForDoctor(TimeSlot wantedTime, DateTime lastDate, Doctor sameSpecDoctor,
        List<Tuple<TimeSlot, Doctor>> nearestThreeSlots, out List<Tuple<TimeSlot, Doctor>> tuples)
    {
        var foundSlots = new List<TimeSlot>();
        for (var i = 0; i < 3; i++)
        {
            var availableTimeSlot =
                FindAvailableTimeslotForOneDoctor(sameSpecDoctor.Email, wantedTime, lastDate, foundSlots);
            if (availableTimeSlot == null)
                break;
            foundSlots.Add(availableTimeSlot);
            nearestThreeSlots.Add(new Tuple<TimeSlot, Doctor>(availableTimeSlot, sameSpecDoctor));
            if (nearestThreeSlots.Count != 3) continue;
            tuples = nearestThreeSlots;
            return true;
        }

        tuples = null;
        return false;
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
        if (CheckListElementsLength(symptoms)) return false;
        if (opinion.Trim().Length < 10) return false;
        if (CheckListElementsLength(allergens)) return false;
        return keyWord.Trim().Length >= 2;
    }

    
    public void ChangePerformingAppointment(int id,Anamnesis anamnesis,List<Prescription> prescriptions,int roomId)
    {
        var appointment = GetAppointmentById(id);
        _scheduleRepository.DeleteAppointment(appointment);
        var performedAppointment = new Appointment(appointment.Id, appointment.Time, appointment.Doctor,
            appointment.PatientEmail, anamnesis,prescriptions);
        performedAppointment.Status = true;
        performedAppointment.Room = roomId;
        _scheduleRepository.InsertAppointment(performedAppointment);
    }

    public bool CheckListElementsLength(List<string> list)
    {
        return list.Any(l => l.Trim().Length < 5);
    }

    public Appointment GetPatientsFirstAppointment(string patientEmail, TimeSlot interval)
    {
        return _scheduleRepository.GetAllAppointments().FirstOrDefault(appointment => appointment.PatientEmail.Equals(patientEmail) && appointment.Time.IsInsideSingleSlot(interval));

    }
    public bool CheckRoomAvailability(int roomId, TimeSlot time)
    {
        foreach (Appointment appointment in _scheduleRepository.GetAllAppointments())
        {
            if (appointment.Room == roomId && time.Overlap(appointment.Time)) return false;

        }
        return true;
    }


}