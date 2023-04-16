using System;
using System.Text.Json.Serialization;

namespace ZdravoCorp.Core.Models.User;

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

    protected bool Equals(Director other)
    {
        return Email == other.Email && FirstName == other.FirstName && LastName == other.LastName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Director)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Email, FirstName, LastName);
    }
}