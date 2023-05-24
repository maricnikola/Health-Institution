using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace ZdravoCorp.Core.Models.SpecialistsRefferals;

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
