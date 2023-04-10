using System;
using System.Collections.Generic;
using System.IO;
using System.Security.RightsManagement;
using System.Text.Json;
using ZdravoCorp.Core.Appointments.Model;
using ZdravoCorp.Core.Equipments.Model;
using ZdravoCorp.Core.MedicalRecords.Model;
using ZdravoCorp.Core.Operations.Model;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.Core.User;

namespace ZdravoCorp.Core.Schedule.Repository;

public class ScheduleRepository
{
    private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {

    };

    private String _fileNameAppointments = "Repository\\appointments.json";
    private String _fileNameOperations = "Repository\\operations.json";
    private List<Appointment> Appointments { get; set; }
    private List<Operation> Operations { get; set; }

    public List<Appointment> GetPatientAppointments(Patient patient)
    {
        List<Appointment> patientAppointments = new List<Appointment>();   
        foreach(Appointment appointment in Appointments)
        {
            if(appointment.MedicalRecord.user == patient) patientAppointments.Add(appointment);
        }
        return patientAppointments;
    }
    public List<Operation> GetPatientOperations(Patient patient)
    {
        List<Operation> patientOperations = new List<Operation>();
        foreach(Operation operation in Operations)
        {
            if (operation.medicalRecord.user == patient) patientOperations.Add(operation);
        }
        return patientOperations;
    }
   
    public List<Appointment> GetDoctorAppointments(Doctor doctor)
    {
        List<Appointment> doctorAppointments = new List<Appointment>();
        foreach(Appointment appointment in Appointments)
        {
            if(appointment.Doctor == doctor) doctorAppointments.Add(appointment);
        }
        return doctorAppointments;
    }

    public List<Operation> GetDoctorOperations(Doctor doctor)
    {
        List<Operation> doctorOperations = new List<Operation>();
        foreach (Operation operation in Operations)
        {
            if (operation.doctor == doctor) doctorOperations.Add(operation);
        }
        return doctorOperations;
    }

    public bool isDoctorAvailable(TimeSlot timeslot, Doctor doctor)
    {
        List<Appointment> appointments = GetDoctorAppointments(doctor);
        List<Operation> operations = GetDoctorOperations(doctor);
        return checkAvailability(appointments, operations, timeslot);
    }

    public bool isPatientAvailable(TimeSlot timeslot, Patient patient)
    {
        List<Appointment> appointments = GetPatientAppointments(patient);
        List<Operation> operations = GetPatientOperations(patient);
        return checkAvailability(appointments, operations, timeslot);
    }

    public bool checkAvailability(List<Appointment> appointments, List<Operation> operations, TimeSlot timeslot)
    {
        foreach (var appointment in appointments)
        {
            if (!appointment.Time.overlap(timeslot) && !appointment.IsCanceled)
                return false;
        }
        foreach (var operation in operations)
        {
            if (!operation.Time.overlap(timeslot) && !operation.IsCanceled)
                return false;
        }
        return true;
    }

    public void LoadApointments()
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

    public void CreateAppointment(TimeSlot time, Doctor doctor, MedicalRecord medicalRecord)
    {
        if (isDoctorAvailable(time,doctor) && isPatientAvailable(time, medicalRecord.user))
        {
            Appointment appointment = new Appointment(0, time, doctor, medicalRecord);
            Appointments.Add(appointment);
            SaveAppointments();
        }
    }
    public void CreateOperation(TimeSlot time, Doctor doctor, MedicalRecord medicalRecord)
    {
        if (isDoctorAvailable(time, doctor) && isPatientAvailable(time, medicalRecord.user))
        {
            Operation operation = new Operation(0, time, doctor, medicalRecord);
            Operations.Add(operation);
            SaveOperations();
        }
    }

    public void CancelAppointment(Appointment appointment)
    {
        bool isOnTime = appointment.Time.GetTimeBeforeStart(DateTime.Now)<24;
        if (Appointments.Contains(appointment) && isOnTime)
        {
            int index = Appointments.IndexOf(appointment);
            appointment.IsCanceled = true;
            Appointments[index] = appointment;
            SaveAppointments();
        }
    }

}