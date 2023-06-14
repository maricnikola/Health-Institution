using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Models;

public class AnnualLeaveDTO
{
    public string Reason { get; set; }
    public TimeSlot Time { get; set; }
    public int Id { get; set; }
    public string DoctorMail { get; set; }
    public AnnualLeave.Status RequestStatus { get; set; }

    public AnnualLeaveDTO(AnnualLeave annualLeave)
    {
        Reason = annualLeave.Reason;
        Time = annualLeave.Time;
        Id = annualLeave.Id;
        DoctorMail = annualLeave.DoctorMail;
        RequestStatus = annualLeave.RequestStatus;
    }
    public AnnualLeaveDTO(string reason, TimeSlot time,string doctorMail)
    {
        Reason = reason;
        Time = time;
        RequestStatus = AnnualLeave.Status.Pending;
        Id = IDGenerator.GetId();
        DoctorMail = doctorMail;
    }
}