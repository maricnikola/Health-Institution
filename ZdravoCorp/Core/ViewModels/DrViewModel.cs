using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.ViewModels;

public class DrViewModel
{
    private readonly Doctor _doctor;

    public DrViewModel(Doctor doctor)
    {
        _doctor = doctor;
    }

    public string DoctorName => _doctor.FullName;
    public string DoctorsName => _doctor.FirstName;
    public string DoctorLastName => _doctor.LastName;
    public string Specialization => _doctor.Specialization.ToString();
    public double DoctorsAverageGrade => _doctor.Grade;

    public string Email => _doctor.Email;

}