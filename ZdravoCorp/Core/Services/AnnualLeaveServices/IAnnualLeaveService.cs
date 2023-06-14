﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.AnnualLeaves;
using ZdravoCorp.Core.Models.HospitalRefferals;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Services.AnnualLeaveServices;

public interface IAnnualLeaveService
{
    public List<AnnualLeave>? GetAll();
    public AnnualLeave? GetById(int id);
    public AnnualLeaveDTO AddAnnualLeave(AnnualLeaveDTO annualLeave);
    public void Delete(int id);
    public void Approve(int id);
    public void Deny(int id);
    public bool CheckAnnualLeaveData(string reason, TimeSlot time);
    public bool CheckAnnualLeaveForDeny(AnnualLeave annualLeave);
    public bool DenyByDoctor(int id);
    public event EventHandler DataChanged;
}
