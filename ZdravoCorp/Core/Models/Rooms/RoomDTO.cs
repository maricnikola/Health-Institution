namespace ZdravoCorp.Core.Models.Rooms;

public class RoomDTO
{
    public int Id { get; set; }
    public RoomType Type { get; set; }
    
    public bool IsUnderRenovation { get; set; }


    public RoomDTO(int id, RoomType type, bool isUnderRenovation)
    {
        Id = id;
        Type = type;
        IsUnderRenovation = isUnderRenovation;
    }
}