using System.Collections.Generic;
using ZdravoCorp.Core.MedicalRecords.Model;

namespace ZdravoCorp.Core.MedicalRecords.Repository;

public class MedicalRecordRepository
{
    private List<MedicalRecord> records { get; set; }
    public void AddRecord(MedicalRecord newMedicalRecord)
    {
        records.Add(newMedicalRecord);
    }
    public void RemoveById(int id)
    {

    }
}