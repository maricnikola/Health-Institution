using System.Windows;

namespace ZdravoCorp.Core.User;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    private string _password;


    public User(string password, int id, string userName, string firstName, string lastName)
    {
        _password = password;
        Id = id;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
    }
    
    public User(){}
}