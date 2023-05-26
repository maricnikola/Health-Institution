using ZdravoCorp.Core.Models.Renovation;

namespace ZdravoCorp.Core.Services.RenovationServices;

public interface IManageRenovationService
{
    public bool StartRenovation(int roomId);
    public bool End(int roomId);
    public bool EndWithSplit(RenovationDTO renovationDto);
    public bool EndWithJoin(RenovationDTO renovationDto);
}