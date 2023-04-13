using System.Text.Json.Serialization;

namespace ZdravoCorp.Core.Models.User;

public class Doctor

{

    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("firstname")] public string FirstName { get; set; }
    [JsonPropertyName("lastname")] public string LastName { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SpecializationType Specialization { get; set; }

    public string FullName => string.Format("Dr {0} {1}", FirstName, LastName);

    [JsonConstructor]
    public Doctor(string email, string firstName, string lastName, SpecializationType specialization)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Specialization = specialization;
    }


    public dynamic GetDoctorForSerialization()
    {
        return new
        {
            email = Email,
            firstname = FirstName,
            lastname = LastName,
            specialization = Specialization
        };
    }

    public enum SpecializationType
    {
        Psychologist,
        Surgeon,
        Neurologist,
        Urologist,
        Anesthesiologist
    }
}