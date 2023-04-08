using System.Collections.Generic;
using ZdravoCorp.Core.Appointments.Model;
using ZdravoCorp.Core.Operations.Model;

namespace ZdravoCorp.Core.Schedule.Repository;

public class ScheduleRepository
{
    private List<Appointment> Appointments { get; set; }
    private List<Operation> Operations { get; set; }
    
}