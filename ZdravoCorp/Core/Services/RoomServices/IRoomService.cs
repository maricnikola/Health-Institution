using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Models.Rooms;

namespace ZdravoCorp.Core.Services.RoomServices;

public interface IRoomService
{
    public List<Room>? GetAll();
    public Room? GetById(int id);
    public void AddRoom(RoomDTO roomDto);

    public void Update(int id, RoomDTO roomDto);

    public void Delete(int id);
    IEnumerable<Room> GetAllExcept(int roomId);
    public event EventHandler DataChanged;
    public int[] GetAllIds();
}