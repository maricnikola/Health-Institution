using System;
using ZdravoCorp.Core.Models.Rooms;

namespace ZdravoCorp.Core.Models.Renovation;

public class RenovationDTO
{
    public int Id { get; set; }
    public Room Room { get; set; }
    public DateTime Start { get; set; }
    public DateTime Until { get; set; }
    public Room Split { get; set; }
    public Room Join { get; set; }
    public Renovation.RenovationStatus Status { get; set; }


    public RenovationDTO(int id, Room room, DateTime start, DateTime until, Room split, Room join, Renovation.RenovationStatus status)
    {
        Id = id;
        Room = room;
        Start = start;
        Until = until;
        Split = split;
        Join = join;
        Status = status;
    }
}