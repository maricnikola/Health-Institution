namespace ZdravoCorp.Core.HospitalSystem.Users.Models;

public class NurseDTO
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public NurseDTO(string email, string firstName, string lastName)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}