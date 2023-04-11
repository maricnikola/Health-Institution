using System.Text.Json.Serialization;

namespace ZdravoCorp.Core.User;

public class Director
{
    [JsonPropertyName("email")] public new string Email { get; set; }
    [JsonPropertyName("firstname")] public string FirstName { get; set; }
    [JsonPropertyName("lastname")] public string LastName { get; set; }
    
    public string FullName => string.Format("Nurse {0} {1}", FirstName, LastName);

    
    [JsonConstructor]
    public Director(string email, string firstName, string lastName)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }


    public dynamic GetDirectorForSerialization()
    {
        return new
        {
            email = Email,
            firstname = FirstName,
            lastname = LastName,
        };
    }
}