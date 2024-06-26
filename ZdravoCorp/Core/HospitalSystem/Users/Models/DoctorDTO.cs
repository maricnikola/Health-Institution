﻿using static ZdravoCorp.Core.HospitalSystem.Users.Models.Doctor;

namespace ZdravoCorp.Core.HospitalSystem.Users.Models;

public class DoctorDTO
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public SpecializationType Specialization { get; set; }
    public string FullName { get; set; }
    public double Grade { get; set; }

    public DoctorDTO(string email, string firstName, string lastName, SpecializationType specialization, double grade)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Specialization = specialization;
        FullName = $"Dr {FirstName} {LastName}";
        Grade = grade;
    }

}