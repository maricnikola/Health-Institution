using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.MedicalRecords;
using ZdravoCorp.Core.Models.Operations;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.ScheduleRepo;

public interface IScheduleRepository
{
    string FileName();
    IEnumerable<object>? GetList();
    void Import(JToken token);
    void AddAppointment(Appointment appointment);
    void AddOperation(Operation operation);
    Operation? GetOperationById(int id);
    Appointment GetAppointmentById(int id);
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
    void CancelAppointment(Appointment appointment);
    Appointment CancelAppointmentByDoctor(Appointment appointment);
    bool IsAppointmentInList(Appointment appointment);
    List<Appointment> GetAppointmentsForShow(DateTime date);
    bool IsForShow(Appointment appointment, DateTime date);

    TimeSlot? FindAvailableTimeslotsForOneDoctor(string doctorsMail, TimeSlot wantedTime, DateTime lastDate,
        List<TimeSlot>? alreadyUsed = null);

    List<Appointment> FindAppointmentsByDoctorPriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        string patientMail);

    List<Appointment> FindAppointmentsByTimePriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        string patientMail, DoctorRepository doctorRepository);

    bool IsPatientExamined(Patient patient, Doctor doctor);
    bool CanPerformAppointment(int id);

    bool CheckPerformingAppointmentData(List<string> symptoms, string opinion, List<string> allergens,
        string keyWord);

    void ChangePerformingAppointment(int id, List<string> symptoms, string opinion, List<string> allergens,
        string keyWord, int roomId);

    Appointment GetPatientsFirstAppointment(string patientEmail, TimeSlot interval);
}