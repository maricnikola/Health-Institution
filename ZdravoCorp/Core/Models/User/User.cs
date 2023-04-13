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
}