using System;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Models.Renovation;

public class RenovationDTO
{
    public int Id { get; set; }
    public Room Room { get; set; }
    public TimeSlot Slot { get; set; }
    public Room? Split { get; set; }
    public Room? Join { get; set; }
    public Renovation.RenovationStatus Status { get; set; }


    public RenovationDTO(int id, Room room, TimeSlot slot, Renovation.RenovationStatus status,  Room? split=null, Room? join=null)
    {
        Id = id;
        Room = room;
        Slot = slot;
        Split = split;
        Join = join;
        Status = status;
    }
}