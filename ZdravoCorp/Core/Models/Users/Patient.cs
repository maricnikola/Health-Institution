using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Models.Users;

public class Patient
{
    [JsonConstructor]
    public Patient(string email, string firstName, string lastName, TimeSpan notificationTime)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        NotificationTime = notificationTime;
    }

    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public TimeSpan NotificationTime { get; set; }

    [JsonIgnore] public string FullName => string.Format("{0} {1}", FirstName, LastName);

    public Patient(PatientDTO patientDto)
    {
        Email=patientDto.Email;
        FirstName = patientDto.FirstName;
        LastName = patientDto.LastName;
    }


    protected bool Equals(Patient other)
    {
        return Email == other.Email && FirstName == other.FirstName && LastName == other.LastName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Patient)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Email, FirstName, LastName);
    }
}