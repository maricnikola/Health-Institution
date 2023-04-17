using System.Collections.Generic;
using ZdravoCorp.Core.Models.User;

namespace ZdravoCorp.Core.Models.MedicalRecord;

public class MedicalRecord
{
    public Patient user;
    public int height { get; set; }
    public int weight { get; set; }
    public List<string> deseaseHistory { get; set; }

    public MedicalRecord()
    {
        return;
    }

    public MedicalRecord(Patient patient)
    {
        user = patient;
        height = 0;
        weight = 0;
        deseaseHistory=new List<string>();
    }
    public MedicalRecord(Patient patient, int h, int w)
    {
        user = patient;
        height = h;
        weight = w;
        deseaseHistory = new List<string>();
    }
    public MedicalRecord(Patient user, int height, int weight, List<string> deseaseHistory)
    {
        user = user;
        height = height;
        weight = weight;
        deseaseHistory = deseaseHistory;
    }

    public override string ToString()
    {
        return "Patient : " + user.ToString() + "height : " + height + "weight : " + weight;
    }
}