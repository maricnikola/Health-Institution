using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Services.NurseServices;

public interface INurseService
{
    public Nurse? GetByEmail(string email);
}