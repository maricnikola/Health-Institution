using System.Windows;

namespace ZdravoCorp.Core.User;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    private string _password;


    public User(string password, int id, string email, string firstName, string lastName)
    {
        _password = password;
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
    
    public User(){}
}