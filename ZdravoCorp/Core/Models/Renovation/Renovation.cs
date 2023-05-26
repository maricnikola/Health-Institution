using System;
using Newtonsoft.Json;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Models.Renovation;

public class Renovation
{
    public int Id { get; set; }
    public Room Room { get; set; }
    public TimeSlot Slot { get; set; }
    public Room? Split { get; set; }
    public Room? Join { get; set; }
    public RenovationStatus Status { get; set; }

    [JsonConstructor]
    public Renovation(int id,Room room, TimeSlot slot,  RenovationStatus status,Room? split=null, Room? join=null)
    {
        Id = id;
        Room = room;
        Slot = slot;
        Split = split;
        Join = join;
        Status = status;
    }

    public Renovation(RenovationDTO renovationDto)
    {
        Id = renovationDto.Id;
        Room = renovationDto.Room;
        Slot = renovationDto.Slot;
        Split = renovationDto.Split;
        Join = renovationDto.Join;
        Status = renovationDto.Status;
    }

    public enum RenovationStatus
    {
        Pending,
        InProgress,
        Finished,
        Failed
    }
}