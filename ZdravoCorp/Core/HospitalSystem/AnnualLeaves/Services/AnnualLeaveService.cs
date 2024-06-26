﻿using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Models;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Repositories;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Services;

public class AnnualLeaveService: IAnnualLeaveService
{
    private IAnnualLeaveRepository _annualLeaveRepository;

    public AnnualLeaveService(IAnnualLeaveRepository annualLeaveRepository)
    {
        _annualLeaveRepository = annualLeaveRepository;
    }

    public List<AnnualLeave>? GetAll()
    {
        return _annualLeaveRepository.GetAll() as List<AnnualLeave>;
    }

    public AnnualLeave? GetById(int id)
    {
        return _annualLeaveRepository.GetById(id);
    }

    public void AddAnnualLeave(AnnualLeaveDTO annualLeave)
    {
        _annualLeaveRepository.Insert(new AnnualLeave(annualLeave.Reason, annualLeave.Time, annualLeave.Id, annualLeave.DoctorMail, annualLeave.RequestStatus));
        
    }

    public void Delete(int id)
    {
        _annualLeaveRepository.Delete(GetById(id));
    }

    public void Approve(int id)
    {
        _annualLeaveRepository.UpdateStatus(id, AnnualLeave.Status.Approved);
        DataChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Deny(int id)
    {
        _annualLeaveRepository.UpdateStatus(id, AnnualLeave.Status.Denied);
        DataChanged?.Invoke(this, EventArgs.Empty);
    }
    public bool CheckAnnualLeaveData(string reason,TimeSlot time)
    {
        if (time.Start > time.End) return false;
        bool hasOverlap = _annualLeaveRepository.GetAll().Any(annualLeave => time.Overlap(annualLeave.Time) &&
        annualLeave.RequestStatus != AnnualLeave.Status.Denied);
        if (reason.Length < 5 || hasOverlap) return false;
        DateTime now = DateTime.Now;
        TimeSpan dateDifference = time.Start.Subtract(now);
        int days = dateDifference.Days;
        if (days < 2) return false;
        return true;
    }
    public bool CheckAnnualLeaveForDeny(AnnualLeave annualLeave)
    {
        if (annualLeave.RequestStatus.Equals(AnnualLeave.Status.Denied) ||
            annualLeave.RequestStatus.Equals(AnnualLeave.Status.Approved)) return false;
        return true;
    }
    public void DenyByDoctor(int id)
    {
        AnnualLeave annualLeave = GetById(id);
        
        _annualLeaveRepository.Delete(annualLeave);
    }
    

    public event EventHandler? DataChanged;
}
