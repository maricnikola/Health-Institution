using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Scheduling.Models;

namespace ZdravoCorp.Core.Scheduling.Repositories;

public interface IScheduleRepository
{
    string FileName();
    IEnumerable<object>? GetList();
    void Import(JToken token);
    void InsertAppointment(Appointment appointment);
    void InsertOperation(Operation operation);
    List<Appointment> GetAllAppointments();
    List<Operation> GetAllOperations();
    void DeleteAppointment(Appointment appointment);
    
}