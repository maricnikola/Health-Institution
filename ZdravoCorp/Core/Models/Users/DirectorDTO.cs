using System.Security.Cryptography;

namespace ZdravoCorp.Core.Models.Users;

public class DirectorDTO
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public DirectorDTO(Director director)
    {
        Email = director.Email;
        FirstName = director.FirstName;
        LastName = director.LastName;
    }
}