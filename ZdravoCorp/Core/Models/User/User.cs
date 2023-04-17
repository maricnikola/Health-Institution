using System;
using System.Text.Json.Serialization;

namespace ZdravoCorp.Core.Models.User;

public class User
{
    [JsonConverter(typeof(JsonStringEnumConverter))] public UserType Type { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("password")] public string? Password { private get;  set; }
    [JsonConverter(typeof(JsonStringEnumConverter))] public State UserState { get; set; }
    

    [JsonConstructor]
    public User(string? password, string email, UserType type, State userState)
    {
        Password = password;
        Email = email;
        UserState = userState;
        Type = type;
    }
    
    public User(string email, UserType type, State userState)
    {
        Email = email;
        UserState = userState;
    }
    

    public dynamic GetUserForSerialization()
    {
        return new
        {
            type = Type.ToString(),
            email = Email,
            password = Password,
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

    protected bool Equals(User other)
    {
        return Type == other.Type && Email == other.Email && Password == other.Password && UserState == other.UserState;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((User)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Type, Email, Password, (int)UserState);
    }
}