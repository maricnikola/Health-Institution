using static ZdravoCorp.Core.Models.Users.User;

namespace ZdravoCorp.Core.Models.Users;

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