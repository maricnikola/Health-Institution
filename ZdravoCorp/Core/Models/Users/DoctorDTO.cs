using static ZdravoCorp.Core.Models.Users.Doctor;

namespace ZdravoCorp.Core.Models.Users;

public class DoctorDTO
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public SpecializationType Specialization { get; set; }
    public string FullName { get; set; }


    public DoctorDTO(Doctor doctor)
    {
        Email=doctor.Email;
        FirstName=doctor.FirstName;
        LastName=doctor.LastName;
        Specialization=doctor.Specialization;
        FullName=doctor.FullName;
    }

}