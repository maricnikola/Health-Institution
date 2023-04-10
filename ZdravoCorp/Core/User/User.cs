using System.Windows;

namespace ZdravoCorp.Core.User;

public class User
{
    public UserType Type { get; set; }
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public State UserState { get; set; }
    
    private readonly string? _password; 


    public User(string password, int id, string email, string firstName, string lastName)
    {
        _password = password;
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        UserState = State.NotBlocked;
    }

    protected dynamic GetUserForSerialization()
    {
        return new
        {
            type = Type.ToString(),
            email = Email,
            password = _password,
            firstname = FirstName,
            lastname = LastName,
            status = UserState.ToString()
        };
    }

    public bool ValidatePassword(string password)
    {
        return _password == password;
    }

    protected User()
    {
    }

    public enum State
    {
        BlockedBySystem,
        BlockedBySecretary,
        NotBlocked
    }

    public enum UserType
    {
        Director,
        Doctor,
        Patient,
        Nurse
    }
}