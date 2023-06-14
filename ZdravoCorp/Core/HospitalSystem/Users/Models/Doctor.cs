using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.HospitalSystem.Users.Models;

public class Doctor
{
    public enum SpecializationType
    {
        Psychologist,
        Surgeon,
        Neurologist,
        Urologist,
        Anesthesiologist
    }

    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public SpecializationType Specialization { get; set; }
    public double Grade { get; set; }
    public string DiscordToken { get; set; }


    [JsonConstructor]
    public Doctor(string email, string firstName, string lastName, SpecializationType specialization, double grade, string discordToken)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Specialization = specialization;
        Grade = grade;
        DiscordToken = discordToken;
    }

    public Doctor(DoctorDTO doctorDto)
    {
        Email=doctorDto.Email;
        FirstName = doctorDto.FirstName;
        LastName = doctorDto.LastName;
        Specialization = doctorDto.Specialization;
        Grade = doctorDto.Grade;
    }



    [JsonIgnore] public string FullName => string.Format("Dr {0} {1}", FirstName, LastName);

    protected bool Equals(Doctor other)
    {
        return Email == other.Email && FirstName == other.FirstName && LastName == other.LastName &&
               Specialization == other.Specialization;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Doctor)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Email, FirstName, LastName, (int)Specialization);
    }
}