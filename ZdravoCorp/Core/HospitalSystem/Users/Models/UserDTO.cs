using static ZdravoCorp.Core.HospitalSystem.Users.Models.User;

namespace ZdravoCorp.Core.HospitalSystem.Users.Models;

public class UserDTO
{
    public UserType Type { get; set; }
    public string Email { get; set; }
    public State UserState { get; set; }

    public UserDTO(UserType type, string email, State userState)
    {
        Type = type;
        Email = email;
        UserState = userState;
    }
}