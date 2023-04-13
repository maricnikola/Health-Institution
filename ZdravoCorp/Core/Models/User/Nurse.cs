using System.Text.Json.Serialization;

namespace ZdravoCorp.Core.Models.User;

public class Nurse
{

    [JsonPropertyName("email")] public new string Email { get; set; }
    [JsonPropertyName("firstname")] public string FirstName { get; set; }
    [JsonPropertyName("lastname")] public string LastName { get; set; }
    
    public string FullName => string.Format("Nurse {0} {1}", FirstName, LastName);

    [JsonConstructor]
    public Nurse(string email, string firstName, string lastName)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }


    public dynamic GetNurseForSerialization()
    {
        return new
        {
            email = Email,
            firstname = FirstName,
            lastname = LastName,
        };
    }
}