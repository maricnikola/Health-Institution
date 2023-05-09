using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Repositories.Schedule;

namespace ZdravoCorp.Core.Repositories.MedicalRecord;

public class MedicalRecordRepository
{
    private List<Models.MedicalRecord.MedicalRecord> _records { get; set; }

    public MedicalRecordRepository(List<Appointment> appointments)
    {
        _records = new List<Models.MedicalRecord.MedicalRecord>();
        foreach (Appointment ap in appointments)
        {
            Models.MedicalRecord.MedicalRecord mr = ap.MedicalRecord;
            _records.Add(mr);
        }
    }

    public void AddRecord(Models.MedicalRecord.MedicalRecord? newMedicalRecord)
    {
        _records.Add(newMedicalRecord);
    }

    public Models.MedicalRecord.MedicalRecord? GetById(string id)
    {
        return _records.FirstOrDefault(record => record.user.Email == id);
    }

    public void RemoveById(int id)
    {
    }
}