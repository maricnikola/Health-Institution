using System.Collections.Generic;

namespace ZdravoCorp.Core.Schedule.Repository;

public class ScheduleRepository
{
    private List<Appointment.Model.Appointment> Appointments { get; set; }
    private List<Operation.Model.Operation> Operations { get; set; }
    
}