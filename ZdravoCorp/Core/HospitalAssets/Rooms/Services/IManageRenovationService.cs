using ZdravoCorp.Core.HospitalAssets.Rooms.Models;

namespace ZdravoCorp.Core.HospitalAssets.Rooms.Services;

public interface IManageRenovationService
{
    public bool StartRenovation(int roomId);
    public bool End(int roomId);
    public bool EndWithSplit(RenovationDTO renovationDto);
    public bool EndWithJoin(RenovationDTO renovationDto);
}