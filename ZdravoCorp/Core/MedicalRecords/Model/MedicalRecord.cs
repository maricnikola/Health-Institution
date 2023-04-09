using System.Collections.Generic;
using ZdravoCorp.Core.User;

namespace ZdravoCorp.Core.MedicalRecords.Model;

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