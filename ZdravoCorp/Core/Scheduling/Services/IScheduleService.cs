﻿using System;
using System.Collections.Generic;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.PatientFiles.MedicalRecords.Models;
using ZdravoCorp.Core.PatientFiles.Presriptions.Models;
using ZdravoCorp.Core.Scheduling.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Scheduling.Services;

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
    List<Appointment> GetDoctorAppointmentsInTimeSlot(string doctorsMail, TimeSlot slot);
    List<Operation> GetDoctorOperations(string doctorsMail);
    bool IsDoctorAvailable(TimeSlot timeslot, string doctorsMail);
    bool IsPatientAvailable(TimeSlot timeslot, string patientMail);
    bool CheckAvailability(List<Appointment> appointments, List<Operation> operations, TimeSlot timeslot);
    Appointment? CreateAppointment(TimeSlot time, Doctor doctor, string email,int roomId);
    void CreateOperation(TimeSlot time, Doctor doctor, MedicalRecord medicalRecord);
    Appointment? ChangeAppointment(int id, TimeSlot time, Doctor doctor, string email);
    void CancelAppointment(int id);
    Appointment CancelAppointmentByDoctor(AppointmentDTO appointment);
    bool IsAppointmentInList(Appointment appointment);
    List<Appointment> GetAppointmentsForShow(DateTime date);
    bool IsForShow(Appointment appointment, DateTime date);
    HashSet<TimeSlot> FindOccupiedTimeSlotsForDoctor(string doctorsMail, List<TimeSlot> timeLimitation);
    TimeSlot? FindFirstEmptyTimeSlotForDoctor(HashSet<TimeSlot> doctorsTimeSlots, List<TimeSlot> allDays,
        string doctorsMail, bool isExtended);
    TimeSlot? FindAvailableTimeslotForOneDoctor(string doctorsMail, TimeSlot wantedTime, DateTime lastDate,
        List<TimeSlot>? alreadyUsed = null, bool isExtended=false);

    List<Appointment> FindAppointmentsByDoctorPriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        string patientMail);

    List<Appointment> FindAppointmentsByTimePriority(Doctor doctor, TimeSlot wantedTime, DateTime lastDate,
        string patientMail, IDoctorService doctorService);

    List<TimeSlot> FindAvailableTimeSlotsByDoctorPriority(string doctorMail, TimeSlot wantedTime, DateTime lastDate);

    List<TimeSlot> GetNearestSlotsByDoctorPriority(int howMany, string doctorsMail, TimeSlot wantedTime,
        DateTime lastDate);

    List<Tuple<TimeSlot, Doctor>> FindAvailableTimeSlotsByTimePriority(Doctor doctor, TimeSlot wantedTime,
        DateTime lastDate, IDoctorService doctorService);

    List<Tuple<TimeSlot, Doctor>> GetNearesThreeSlotsByTimePriority(Doctor doctor,
        TimeSlot wantedTime, DateTime lastDate, IDoctorService doctorService);

    bool IsPatientExamined(Patient patient, Doctor doctor);
    bool CanPerformAppointment(int id);

    bool CheckPerformingAppointmentData(List<string> symptoms, string opinion, List<string> allergens,
        string keyWord);

    void ChangePerformingAppointment(int id,Anamnesis anamnesis,List<Prescription> prescriptions,int roomId);

    bool CheckListElementsLength(List<string> list);


    Appointment GetPatientsFirstAppointment(string patientEmail, TimeSlot interval);
    bool CheckRoomAvailability(int roomId, TimeSlot time);

    bool HasAppointmentsAfter(int roomId, DateTime start);

}