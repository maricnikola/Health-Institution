using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.MedicalRecords;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.MedicalRecordRepo;

public class MedicalRecordRepository : ISerializable, IMedicalRecordRepository
{
    private readonly string _filename = @".\..\..\..\Data\medicalRecords.json";

    public MedicalRecordRepository()
    {
        _records = new List<MedicalRecord>();
        Serializer.Load(this);
    }

    private List<MedicalRecord>? _records { get; set; }

    public string FileName()
    {
        return _filename;
    }

    public IEnumerable<object>? GetList()
    {
        return _records;
    }

    public void Import(JToken token)
    {
        _records = token.ToObject<List<MedicalRecord>>();
    }


    public IEnumerable<MedicalRecord> GetAll()
    {
        return _records;
    }

    public void Insert(MedicalRecord newMedicalRecord)
    {
        var index = _records.FindIndex(record => record.user.Equals(newMedicalRecord.user));
        if (index != -1) _records[index] = newMedicalRecord;
        else _records.Add(newMedicalRecord);
    }

    public void Delete(MedicalRecord entity)
    {
        _records.Remove(entity);
    }

    public MedicalRecord GetById(int id)
    {
        throw new System.NotImplementedException();
    }

    public MedicalRecord? GetById(string id)
    {
        return _records.FirstOrDefault(record => record.user.Email == id);
    }

    public void Delete(string id)
    {
        _records.RemoveAll(record => record.user.Email == id);
    }

    public void ChangeRecord(string patientEmail, int newHeight, int newWeight, List<string> newDeseaseHistory)
    {
        var medicalRecordToBeChanged = GetById(patientEmail);

        if (medicalRecordToBeChanged != null)
        {
            medicalRecordToBeChanged.weight = newWeight;
            medicalRecordToBeChanged.height = newHeight;
            medicalRecordToBeChanged.deseaseHistory = newDeseaseHistory;
            Serializer.Save(this);
        }
    }

    public bool CheckDataForChanges(int newWeight, int newHeight, List<string> newDeseaseHistory)
    {
        if (newWeight < 30 && newWeight > 250) return false;
        if (newHeight > 300 && newHeight < 50) return false;
        foreach (var desease in newDeseaseHistory)
            if (desease.Trim().Length < 5)
                return false;
        return true;
    }
}