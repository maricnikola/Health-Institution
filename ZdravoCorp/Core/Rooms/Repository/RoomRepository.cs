using System.Collections.Generic;

namespace ZdravoCorp.Core.Rooms.Repository;

public class RoomRepository
{
    private List<Room> Rooms { get; set; }


    public void Add(Room newRoom)
    {
        Rooms.Add(newRoom);
    }
    public void SaveToFile()
    {
        
    }
}