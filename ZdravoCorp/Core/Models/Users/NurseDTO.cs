namespace ZdravoCorp.Core.Models.Users;

public class NurseDTO
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public NurseDTO(Nurse nurse)
    {
        Email=nurse.Email;
        FirstName=nurse.FirstName;
        LastName=nurse.LastName;
    }

}