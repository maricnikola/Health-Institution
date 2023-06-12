using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Models.AnnualLeaves;

public class AnnualLeave
{
    public string Reason { get; set; }
    public TimeSlot Time { get; set; }
    public int Id { get; set; }
    public string DoctorMail { get; set; }
    public bool Status { get; set; }

    public AnnualLeave(string reason, TimeSlot time, int id, string doctorMail, bool status)
    {
        Reason = reason;
        Time = time;
        Id = id;
        DoctorMail = doctorMail;
        Status = status;
    }
}
