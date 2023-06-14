using System;
using System.Collections.Generic;
using ZdravoCorp.Core.HospitalAssets.Rooms.Models;

namespace ZdravoCorp.Core.HospitalAssets.Rooms.Services;

public interface IRoomService
{
    public List<Room>? GetAll();
    public Room? GetById(int id);
    public void AddRoom(RoomDTO roomDto);

    public void Update(int id, RoomDTO roomDto);
    public bool UpdateRenovation(int id, bool status);

    public void Delete(int id);
    IEnumerable<Room> GetAllExcept(int roomId);
    public event EventHandler DataChanged;
    public int[] GetAllIds();
}