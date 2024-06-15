using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Scheduling.Models;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Counters;

namespace ZdravoCorp.Core.Scheduling.Repositories;

public class ScheduleRepository : ISerializable, IScheduleRepository
{
    private readonly CounterDictionary _counterDictionary;

    private readonly string _fileNameAppointments = @".\..\..\..\..\Data\appointments.json";
    private string _fileNameOperations = @".\..\..\..\..\Data\operations.json";

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

}