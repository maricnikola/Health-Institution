using System.IO;
using System.Windows;
using System;
using System.Text.Json.Serialization;

namespace ZdravoCorp.Core.User;

public class User
{
    [JsonConverter(typeof(JsonStringEnumConverter))] public UserType Type { get; set; }
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("firstname")] public string FirstName { get; set; }
    [JsonPropertyName("lastname")] public string LastName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))] public State UserState { get; set; }
    [JsonPropertyName("password")] public string Password { private get;  set; }



    public User(string password, int id, string email, string firstName, string lastName)
    {
        Password = password;
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        UserState = State.NotBlocked;
    }

    [JsonConstructor]
    public User(string password, int id, string email, string firstName, string lastName, UserType type, State userState)
    {
        Password = password;
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        UserState = userState;
        Type = type;

        /*State state;
        Enum.TryParse(userState, out state);
        UserState = state;
        UserType tp;
        Enum.TryParse(type, out tp);
        Type = tp;*/
    }


    public dynamic GetUserForSerialization()
    {
        return new
        {
            id = Id,
            type = Type.ToString(),
            email = Email,
            password = Password,
            firstname = FirstName,
            lastname = LastName,
            userstate = UserState.ToString()
        };
    }

    public bool ValidatePassword(string password)
    {
        return Password == password;
    }

    public User()
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