using System.Collections.Generic;
using ZdravoCorp.Core.PatientFiles.MedicalRecords.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.PatientFiles.MedicalRecords.Repositories;

public interface IMedicalRecordRepository : IRepository<MedicalRecord>
{
    void ChangeRecord(string patientEmail, int newHeight, int newWeight, List<string> newDeseaseHistory);
    bool CheckDataForChanges(int newWeight, int newHeight, List<string> newDeseaseHistory);

    public string FileName();
    public MedicalRecord? GetById(string id);
}