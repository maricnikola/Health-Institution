using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.HospitalSystem.Users.Models;

public class Nurse
{
    [JsonConstructor]
    public Nurse(string email, string firstName, string lastName, string discordToken)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        DiscordToken = discordToken;
    }

    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DiscordToken { get; set; }


    [JsonIgnore] public string FullName => string.Format("Nurse {0} {1}", FirstName, LastName);

    public Nurse(NurseDTO nurseDto)
    {
        Email=nurseDto.Email;
        FirstName=nurseDto.FirstName;
        LastName=nurseDto.LastName;
    }

    public Nurse(string email, string firstName, string lastName)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    protected bool Equals(Nurse other)
    {
        return Email == other.Email && FirstName == other.FirstName && LastName == other.LastName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Nurse)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Email, FirstName, LastName);
    }
}