using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Repositories.RoomRepo;

namespace ZdravoCorp.Core.Services.RoomServices;

public class RoomService : IRoomService
{
    private IRoomRepository _roomRepository;
    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }
    public List<Room>? GetAll()
    {
        return _roomRepository.GetAll() as List<Room>;
    }

    public Room? GetById(int id)
    {
        return _roomRepository.GetById(id);
    }

    public void AddRoom(RoomDTO roomDto)
    {
        _roomRepository.Insert(new Room(roomDto));
        DataChanged?.Invoke(this, new EventArgs());
    }

    public void Update(int id, RoomDTO roomDto)
    {
        var oldRoom = _roomRepository.GetById(id);
            if (oldRoom == null)
            {
                throw new KeyNotFoundException();
            }
            _roomRepository.Delete(oldRoom);
            _roomRepository.Insert(new Room(roomDto));
            DataChanged?.Invoke(this, new EventArgs());
        

    }

    public bool UpdateRenovation(int id, bool status)
    {
        var room = GetById(id);

        if (room == null)
            throw new KeyNotFoundException();
        if (room.IsUnderRenovation == status)
            return true;
        var roomDto = new RoomDTO(room.Id, room.Type, status);
        Update(id, roomDto);
        DataChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }


    public void Delete(int id)
    {
        _roomRepository.Delete(_roomRepository.GetById(id));
        DataChanged?.Invoke(this, new EventArgs());
    }

    public IEnumerable<Room> GetAllExcept(int roomId)
    {
        return _roomRepository.GetAll().Where(room => room.Id != roomId);
    }

    public int[] GetAllIds()
    {
        return _roomRepository.GetAll().Select(room => room.Id).ToArray();
    }

    public event EventHandler? DataChanged;
}