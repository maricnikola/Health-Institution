using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using ZdravoCorp.Core.Models.MedicalRecords;

namespace ZdravoCorp.Core.Services.MedicalRecordServices;

public interface IMedicalRecordService
{
    public IEnumerable<MedicalRecord> GetAll();

    public void Insert(MedicalRecord newMedicalRecord);

    public void Delete(MedicalRecord entity);

    public MedicalRecord? GetById(string id);

    public void Delete(string id);

    public bool CheckDataForChanges(int newWeight, int newHeight, List<string> newDeseaseHistory);

    public void ChangeRecord(string patientEmail, int newHeight, int newWeight, List<string> newDeseaseHistory);



}