using System;
using System.Collections.Generic;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Services;

public interface IAnnualLeaveService
{
    public List<AnnualLeave>? GetAll();
    public AnnualLeave? GetById(int id);
    public void AddAnnualLeave(AnnualLeaveDTO annualLeave);
    public void Delete(int id);
    public void Approve(int id);
    public void Deny(int id);
    public bool CheckAnnualLeaveData(string reason, TimeSlot time);
    public bool CheckAnnualLeaveForDeny(AnnualLeave annualLeave);
    public void DenyByDoctor(int id);
    public event EventHandler DataChanged;
}
