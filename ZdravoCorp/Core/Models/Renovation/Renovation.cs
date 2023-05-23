using System;
using Newtonsoft.Json;
using ZdravoCorp.Core.Models.Rooms;

namespace ZdravoCorp.Core.Models.Renovation;

public class Renovation
{
    public int Id { get; set; }
    public Room Room { get; set; }
    public DateTime Start { get; set; }
    public DateTime Until { get; set; }
    public Room Split { get; set; }
    public Room Join { get; set; }
    public RenovationStatus Status { get; set; }

    [JsonConstructor]
    public Renovation(int id,Room room, DateTime start, DateTime until, Room split, Room join, RenovationStatus status)
    {
        Id = id;
        Room = room;
        Start = start;
        Until = until;
        Split = split;
        Join = join;
        Status = status;
    }

    public Renovation(RenovationDTO renovationDto)
    {
        Id = renovationDto.Id;
        Room = renovationDto.Room;
        Start = renovationDto.Start;
        Until = renovationDto.Until;
        Split = renovationDto.Split;
        Join = renovationDto.Join;
        Status = renovationDto.Status;
    }

    public enum RenovationStatus
    {
        Pending,
        InProgress,
        Finished
    }
}