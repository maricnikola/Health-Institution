using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.MedicalRecord;

public class MedicalRecordRepository : ISerializable
{
    private List<Models.MedicalRecord.MedicalRecord> records { get; set; }
    private readonly string _filename = @".\..\..\..\Data\medicalRecords.json";
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    public MedicalRecordRepository()
    {
        records = new List<Models.MedicalRecord.MedicalRecord>();
        //LoadFromFile();
        Serializer.Load(this);
    }

    //public MedicalRecordRepository()
    public void LoadFromFile()
    {
        var text = File.ReadAllText(_filename);
        if (text == "")
            throw new EmptyFileException("File is empty!");
        try
        {
            var inventory = JsonSerializer.Deserialize<List<Models.MedicalRecord.MedicalRecord>>(text);
            inventory?.ForEach(record => records.Add(record));
        }
        catch (JsonException e)
        {
            Trace.WriteLine(e);
        }
    }
    public void SaveToFile()
    {
        if (records.Count == 0)
        {
            Trace.WriteLine($"Repository is empty! {this.GetType()}");
            return;
        }
        var _records = JsonSerializer.Serialize(records, _serializerOptions);

        File.WriteAllText(_filename, _records);
    }

    public void AddRecord(Models.MedicalRecord.MedicalRecord newMedicalRecord)
    {
        var index = records.FindIndex(record => record.user.Equals(newMedicalRecord.user));
        if (index != -1)    records[index] = newMedicalRecord;
        else    records.Add(newMedicalRecord);
        
        //return records.FirstOrDefault(record => record.user.Email == id);
    }

    public Models.MedicalRecord.MedicalRecord? GetById(string id)
    {
        return records.FirstOrDefault(record => record.user.Email == id);
    }

    public void RemoveById(string id)
    {
        records.RemoveAll(record => record.user.Email == id);
    }

    public string FileName()
    {
        return _filename;
        //throw new System.NotImplementedException();
    }

    public IEnumerable<object>? GetList()
    {
        //throw new System.NotImplementedException();
        return records;
    }

    public void Import(JToken token)
    {
        //throw new System.NotImplementedException();
        records = token.ToObject<List<Models.MedicalRecord.MedicalRecord>>();
    }
}