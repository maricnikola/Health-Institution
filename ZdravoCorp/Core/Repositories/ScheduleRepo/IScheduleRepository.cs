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
    void InsertAppointment(Appointment appointment);
    void InsertOperation(Operation operation);
    List<Appointment> GetAllAppointments();
    List<Operation> GetAllOperations();
    void DeleteAppointment(Appointment appointment);
    
}