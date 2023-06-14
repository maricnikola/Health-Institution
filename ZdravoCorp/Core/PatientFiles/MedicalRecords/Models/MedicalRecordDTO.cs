using System.Collections.Generic;
using ZdravoCorp.Core.HospitalSystem.Users.Models;

namespace ZdravoCorp.Core.PatientFiles.MedicalRecords.Models;

public class MedicalRecordDTO
{
    public Patient Patient { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public List<string> DiseaseHistory { get; set; }

    public MedicalRecordDTO(Patient patient, int height, int weight, List<string> diseaseHistory)
    {
        Patient = patient;
        Height = height;
        Weight = weight;
        DiseaseHistory = diseaseHistory;
    }
}