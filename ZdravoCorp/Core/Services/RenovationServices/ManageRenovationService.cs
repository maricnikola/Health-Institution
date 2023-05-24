using ZdravoCorp.Core.Models.Renovation;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.RenovationRepo;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.RoomServices;

namespace ZdravoCorp.Core.Services.RenovationServices;

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
        _inventoryService.MoveItemsToStockRoom(renovationDto.Room.Id);
        return true;
    }

    public bool EndWithJoin(RenovationDTO renovationDto)
    {
        if (!_roomService.UpdateRenovation(renovationDto.Room.Id, false))
            return false;
        _inventoryService.MoveItemsToStockRoom(renovationDto.Room.Id);
        _inventoryService.MoveItemsToStockRoom(renovationDto.Join.Id);
        _roomService.Delete(renovationDto.Room.Id);
        return true;
    }
}