namespace ZdravoCorp.Core.User;

public class Doctor:User
{
    public Specialization SpecialityType { get; set; }

    public string FullName => string.Format("Dr. {0} {1}", FirstName, LastName);
}

public enum Specialization
{
    psychologist,
    surgeon,
    neurologist,
    urologist,
    anesthesiologist
}