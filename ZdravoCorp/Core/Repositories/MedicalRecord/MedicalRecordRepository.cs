using System.Collections.Generic;

namespace ZdravoCorp.Core.Repositories.MedicalRecord;

public class MedicalRecordRepository
{
    private List<Models.MedicalRecord.MedicalRecord> records { get; set; }
    public void AddRecord(Models.MedicalRecord.MedicalRecord newMedicalRecord)
    {
        records.Add(newMedicalRecord);
    }
    public void RemoveById(int id)
    {

    }
}