using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Models.User;

public class Director
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [JsonIgnore] public string FullName => string.Format("Nurse {0} {1}", FirstName, LastName);


    [JsonConstructor]
    public Director(string email, string firstName, string lastName)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
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