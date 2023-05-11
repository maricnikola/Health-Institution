using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using ZdravoCorp.Core.Models.Users;

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
        this.user = patient;
        height = 0;
        weight = 0;
        deseaseHistory=new List<string>();
    }
    public MedicalRecord(Patient patient, int h, int w)
    {
        this.user = patient;
        height = h;
        weight = w;
        deseaseHistory = new List<string>();
    }
    public MedicalRecord(Patient patient, int height, int weight, List<string> deseaseHistory)
    {
        this.user = patient;
        this.height = height;
        this.weight = weight;
        this.deseaseHistory = deseaseHistory;
    }

    public override string ToString()
    {
        return "Patient : " + user.ToString() + "height : " + height + "weight : " + weight;
    }

    public string DiseaseHistoryToString()
    {
        string result = deseaseHistory.Any() ? string.Join(",", deseaseHistory) : string.Empty;
        return result;
    }
}