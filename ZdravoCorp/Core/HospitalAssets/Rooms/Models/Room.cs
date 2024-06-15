using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.HospitalAssets.Rooms.Models;

public class Room
{
    [JsonConstructor]
    public Room(int id, RoomType type, bool isUnderRenovation)
    {
        Id = id;
        Type = type;
        IsUnderRenovation = isUnderRenovation;
    }

    public Room(RoomDTO roomDto)
    {
        Id=roomDto.Id;
        Type = roomDto.Type;
        IsUnderRenovation = roomDto.IsUnderRenovation;
    }

    public int Id { get; set; }
    public RoomType Type { get; set; }
    
    public bool IsUnderRenovation { get; set; }

    protected bool Equals(Room other)
    {
        return Id == other.Id && Type == other.Type && IsUnderRenovation == other.IsUnderRenovation;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Room)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, (int)Type, IsUnderRenovation);
    }
}

public enum RoomType
{
    StockRoom,
    OperationRoom,
    ExaminationRoom,
    PatientRoom,
    WaitingRoom
}