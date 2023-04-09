namespace ZdravoCorp.Core.User;

public class Doctor:User
{
    public Specialization SpecialityType { get; set; }

}

public enum Specialization
{
    psychologist,
    surgeon,
    neurologist,
    urologist,
    anesthesiologist
}