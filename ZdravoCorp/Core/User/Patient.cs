namespace ZdravoCorp.Core.User;

public class Patient : User
{

    public string FullName => string.Format("{0} {1}", FirstName, LastName);

}