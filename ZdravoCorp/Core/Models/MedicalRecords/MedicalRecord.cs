using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Models.MedicalRecords;

public class MedicalRecord
{
    public Patient Patient { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public List<string> DiseaseHistory { get; set; }

    
    public MedicalRecord(Patient patient)
    {
        Patient = patient;
        Height = 0;
        Weight = 0;
        DiseaseHistory = new List<string>();
    }

    public MedicalRecord(Patient patient, int h, int w)
    {
        Patient = patient;
        Height = h;
        Weight = w;
        DiseaseHistory = new List<string>();
    }

    [JsonConstructor]
    public MedicalRecord(Patient patient, int height, int weight, List<string> diseaseHistory)
    {
        Patient = patient;
        Height = height;
        Weight = weight;
        DiseaseHistory = diseaseHistory;
    }



    public override string ToString()
    {
        return "Patient : " + Patient + "Height : " + Height + "Weight : " + Weight;
    }

    public string DiseaseHistoryToString()
    {
        var result = DiseaseHistory.Any() ? string.Join(",", DiseaseHistory) : string.Empty;
        return result;
    }

    public MedicalRecord(MedicalRecordDTO medicalRecordDto)
    {
        DiseaseHistory = medicalRecordDto.DiseaseHistory;
        Patient = medicalRecordDto.Patient;
        Height = medicalRecordDto.Height;
        Weight = medicalRecordDto.Weight;
    }
}