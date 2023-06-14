﻿using ZdravoCorp.Core.Models.AnnualLeaves;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class AnnualLeaveRequestViewModel : ViewModelBase
{
    private AnnualLeave _annualLeave;

    public string Reason => _annualLeave.Reason;
    public string StartTime => _annualLeave.Time.Start.ToString();
    public string EndTime => _annualLeave.Time.End.ToString();
    public string Doctor => _annualLeave.DoctorMail;
    public string Status => _annualLeave.RequestStatus.ToString();
    public int Id => _annualLeave.Id;

    public AnnualLeaveRequestViewModel(AnnualLeave annualLeave)
    {
        _annualLeave = annualLeave;
    }
}