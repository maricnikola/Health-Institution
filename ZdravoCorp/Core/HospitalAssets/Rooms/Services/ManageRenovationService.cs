using ZdravoCorp.Core.HospitalAssets.Rooms.Models;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;

namespace ZdravoCorp.Core.HospitalAssets.Rooms.Services;

public class ManageRenovationService : IManageRenovationService
{
    private IRenovationService _renovationService;
    private IRoomService _roomService;
    private IInventoryService _inventoryService;
    public ManageRenovationService(IRenovationService renovationService, IRoomService roomService, IInventoryService inventoryService)
    {
        _renovationService = renovationService;
        _roomService = roomService;
        _inventoryService = inventoryService;
    }
    public bool StartRenovation(int roomId)
    {
        return _roomService.UpdateRenovation(roomId, true);
    }

    public bool End(int roomId)
    {
        return _roomService.UpdateRenovation(roomId, false);
    }

    public bool EndWithSplit(RenovationDTO renovationDto)
    {
        if (!_roomService.UpdateRenovation(renovationDto.Room.Id, false))
            return false;
        _roomService.AddRoom(new RoomDTO(renovationDto.Split.Id, renovationDto.Split.Type, false));
        _inventoryService.MoveItemsToStockRoom(renovationDto.Room.Id, _roomService.GetById(999));
        return true;
    }

    public bool EndWithJoin(RenovationDTO renovationDto)
    {
        if (!_roomService.UpdateRenovation(renovationDto.Room.Id, false))
            return false;
        _inventoryService.MoveItemsToStockRoom(renovationDto.Room.Id, _roomService.GetById(999));
        _inventoryService.MoveItemsToStockRoom(renovationDto.Join.Id,_roomService.GetById(999));
        _roomService.UpdateRenovation(renovationDto.Join.Id, false);
        _roomService.Delete(renovationDto.Room.Id);
        return true;
    }
}