namespace ZdravoCorp.Core.Models.Rooms;

public class RoomDTO
{
    public int Id { get; set; }
    public RoomType Type { get; set; }


    public RoomDTO(int id, RoomType type)
    {
        Id = id;
        Type = type;
    }
}