using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using ZdravoCorp.Core.Counters;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.Operation;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.TimeSlots;

namespace ZdravoCorp.Core.Repositories.Schedule;

public class ScheduleRepository
{
    private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {

    };

    private String _fileNameAppointments = @".\..\..\..\Data\appointments.json";
    private String _fileNameOperations = @".\..\..\..\Data\operations.json";
    private List<Appointment> Appointments { get; set; }
    private List<Operation> Operations { get; set; }
    private CounterDictionary _counterDictionary;

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

    public Appointment GetAppointmentById(int id)
    {
        foreach (Appointment appointment in Appointments)
        {
            if (appointment.Id==id) 
                return appointment;
        }

        return null;
    }

    public void AddOperation(Operation operation)
    {
        Operations.Add(operation);
    }

    public Operation? GetOperationById(int id)
    {
        return Operations.FirstOrDefault(op => op.Id == id);
    }

    public List<Appointment> GetPatientAppointments(Patient patient)
    {
        List<Appointment> patientAppointments = new List<Appointment>();   
        foreach(Appointment appointment in Appointments)
        {
            if(appointment.MedicalRecord.user.Email == patient.Email && !appointment.IsCanceled) patientAppointments.Add(appointment);
        }
        return patientAppointments;
    }
    public List<Operation> GetPatientOperations(Patient patient)
    {
        List<Operation> patientOperations = new List<Operation>();
        foreach(Operation operation in Operations)
        {
            if (operation.MedicalRecord.user.Email == patient.Email) patientOperations.Add(operation);
        }
        return patientOperations;
    }
   
    public List<Appointment> GetDoctorAppointments(Doctor doctor)
    {
        List<Appointment> doctorAppointments = new List<Appointment>();
        foreach(Appointment appointment in Appointments)
        {
            if(appointment.Doctor.Email == doctor.Email) doctorAppointments.Add(appointment);
        }
        return doctorAppointments;
    }

    public List<Operation> GetDoctorOperations(Doctor doctor)
    {
        List<Operation> doctorOperations = new List<Operation>();
        foreach (Operation operation in Operations)
        {
            if (operation.Doctor.Email == doctor.Email) doctorOperations.Add(operation);
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
        if (isDoctorAvailable(time,doctor) && isPatientAvailable(time, medicalRecord.user) && time.start>DateTime.Now)
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
        if (isDoctorAvailable(time, doctor) && isPatientAvailable(time, medicalRecord.user))
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
                if (isDoctorAvailable(time, doctor) && isPatientAvailable(time, medicalRecord.user))
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
        bool isOnTime = appointment.Time.GetTimeBeforeStart(DateTime.Now)<24;
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

}