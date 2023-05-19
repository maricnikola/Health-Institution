using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.MedicalRecords;
using ZdravoCorp.Core.Models.Operations;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Services.ScheduleServices;

public interface IScheduleService
{
    void AddAppointment(AppointmentDTO appointment);
    void AddOperation(OperationDTO operation);
    Operation? GetOperationById(int id);
    Appointment? GetAppointmentById(int id);
    List<Appointment> GetAllAppointments();
    List<Appointment> GetPatientAppointments(string patientMail);
    List<Operation> GetPatientOperations(string patientMail);
    List<Appointment> GetPatientsOldAppointments(string patientMail);
    List<Appointment> GetDoctorAppointments(string doctorsMail);
    List<Operation> GetDoctorOperations(string doctorsMail);
    bool isDoctorAvailable(TimeSlot timeslot, string doctorsMail);
    bool isPatientAvailable(TimeSlot timeslot, string patientMail);
    bool checkAvailability(List<Appointment> appointments, List<Operation> operations, TimeSlot timeslot);
    Appointment? CreateAppointment(TimeSlot time, Doctor doctor, string email);
    void CreateOperation(TimeSlot time, Doctor doctor, MedicalRecord medicalRecord);
    Appointment? ChangeAppointment(int id, TimeSlot time, Doctor doctor, string email);
    void CancelAppointment(AppointmentDTO appointment);
    Appointment CancelAppointmentByDoctor(AppointmentDTO appointment);
    bool IsAppointmentInList(Appointment appointment);
    List<Appointment> GetAppointmentsForShow(DateTime date);
    bool IsForShow(Appointment appointment, DateTime date);
    HashSet<TimeSlot> FindOccupiedTimeSlotsForDoctor(string doctorsMail, List<TimeSlot> timeLimitation);
    TimeSlot? FindFirstEmptyTimeSlotForDoctor(HashSet<TimeSlot> doctorsTimeSlots, List<TimeSlot> allDays,
        string doctorsMail);
    TimeSlot? FindAvailableTimeslotsForOneDoctor(string doctorsMail, TimeSlot wantedTime, DateTime lastDate,
        List<TimeSlot>? alreadyUsed = null);

    List<Appointment> FindAppointmentsByDoctorPriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        string patientMail);

    List<Appointment> FindAppointmentsByTimePriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        string patientMail, DoctorRepository doctorRepository);

    List<TimeSlot> FindAvailableTimeSlotsByDoctorPriority(string doctorMail, TimeSlot wantedTime, DateTime lastDate);

    List<TimeSlot> GetNearestSlotsByDoctorPriority(int howMany, string doctorsMail, TimeSlot wantedTime,
        DateTime lastDate);

    List<Tuple<TimeSlot, Doctor>> FindAvailableTimeSlotsByTimePriority(Doctor doctor, TimeSlot wantedTime,
        DateTime lastDate, DoctorRepository doctorRepository);

    List<Tuple<TimeSlot, Doctor>> GetNearesThreeSlotsByTimePriority(Doctor doctor,
        TimeSlot wantedTime, DateTime lastDate, DoctorRepository doctorRepository);

    bool IsPatientExamined(Patient patient, Doctor doctor);
    bool CanPerformAppointment(int id);

    bool CheckPerformingAppointmentData(List<string> symptoms, string opinion, List<string> allergens,
        string keyWord);

    void ChangePerformingAppointment(int id, List<string> symptoms, string opinion, List<string> allergens,
        string keyWord, int roomId);

    bool checkListElementsLength(List<string> list);


    Appointment GetPatientsFirstAppointment(string patientEmail, TimeSlot interval);

}