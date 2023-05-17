using System.Collections.Generic;
using ZdravoCorp.Core.Models.MedicalRecords;

namespace ZdravoCorp.Core.Repositories.MedicalRecordRepo;

public interface IMedicalRecordRepository : IRepository<MedicalRecord>
{
    void ChangeRecord(string patientEmail, int newHeight, int newWeight, List<string> newDeseaseHistory);
    bool CheckDataForChanges(int newWeight, int newHeight, List<string> newDeseaseHistory);

    public MedicalRecord? GetById(string id);
}