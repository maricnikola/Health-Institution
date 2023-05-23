namespace ZdravoCorp.Core.Models.Users;

public class PatientDTO
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }

    public PatientDTO(string email, string firstName, string lastName, string fullName)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        FullName = $"{FirstName} {LastName}";
    }

}