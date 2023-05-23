﻿using System.Collections.Generic;
using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Models.MedicalRecords;

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