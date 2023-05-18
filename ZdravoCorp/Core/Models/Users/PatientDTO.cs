namespace ZdravoCorp.Core.Models.Users;

public class PatientDTO
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }

    public PatientDTO(Patient patient)
    {
        Email=patient.Email;
        FirstName=patient.FirstName;
        LastName=patient.LastName;
        FullName=patient.FullName;
    }
}