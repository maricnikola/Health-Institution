namespace ZdravoCorp.Core.PatientFiles.Refferals.Models;

public class Therapy
{
    public string Description { get; set; }
    public bool Status { get; set; }

    public Therapy(string description, bool status)
    {
        Description = description;
        Status = status;
    }
}
