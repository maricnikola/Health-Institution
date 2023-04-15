using System.Collections.Generic;
using System.Linq;

namespace ZdravoCorp.Core.Repositories.MedicalRecord;

public class MedicalRecordRepository
{
    private List<Models.MedicalRecord.MedicalRecord> _records { get; set; }
    public void AddRecord(Models.MedicalRecord.MedicalRecord newMedicalRecord)
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