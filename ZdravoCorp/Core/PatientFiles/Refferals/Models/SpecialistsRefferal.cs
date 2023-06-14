using Newtonsoft.Json;

namespace ZdravoCorp.Core.PatientFiles.Refferals.Models;

public class SpecialistsRefferal
{
    public int Id { get; set; }
    public string PatientMail { get; set; } 
    public string DoctorMail { get; set; }

    [JsonConstructor]
    public SpecialistsRefferal(int id,string patientMail, string doctorMail)
    {
        Id = id;
        PatientMail = patientMail;
        DoctorMail = doctorMail;
    }
}
