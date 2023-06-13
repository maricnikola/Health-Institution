using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.AnnualLeaves;
using ZdravoCorp.Core.Models.HospitalRefferals;
using ZdravoCorp.Core.Repositories.AnnualLeavesRepo;
using ZdravoCorp.Core.Repositories.HospitalRefferalsRepo;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Services.AnnualLeaveServices;

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
    }

    public void Deny(int id)
    {
        _annualLeaveRepository.UpdateStatus(id, AnnualLeave.Status.Denied);
    }
}
