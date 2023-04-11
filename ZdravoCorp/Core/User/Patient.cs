using System.Text.Json.Serialization;

namespace ZdravoCorp.Core.User;

public class Patient
{
    [JsonPropertyName("email")] public  string Email { get; set; }
    [JsonPropertyName("firstname")] public string FirstName { get; set; }
    [JsonPropertyName("lastname")] public string LastName { get; set; }
    
    public string FullName => string.Format("Patient: {0} {1}", FirstName, LastName);

    [JsonConstructor]
    public Patient(string email, string firstName, string lastName)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }


    public dynamic GetPatientForSerialization()
    {
        return new
        {
            email = Email,
            firstname = FirstName,
            lastname = LastName,
        };
    }
}