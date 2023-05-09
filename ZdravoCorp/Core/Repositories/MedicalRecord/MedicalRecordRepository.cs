using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.MedicalRecord;

namespace ZdravoCorp.Core.Repositories.MedicalRecord;

public class MedicalRecordRepository
{
    private List<Models.MedicalRecord.MedicalRecord> records { get; set; }
    private readonly string _filename = @".\..\..\..\Data\records.json";
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    public MedicalRecordRepository()
    {
        records = new List<Models.MedicalRecord.MedicalRecord>();
        LoadFromFile();
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

}