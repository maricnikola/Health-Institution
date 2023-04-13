using System.Collections.Generic;

namespace ZdravoCorp.Core.Repositories.Room;

public class RoomRepository
{
    private List<Models.Room.Room> Rooms { get; set; }


    public void Add(Models.Room.Room newRoom)
    {
        Rooms.Add(newRoom);
    }
    public void SaveToFile()
    {
        
    }
}