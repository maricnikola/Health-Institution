namespace ZdravoCorp.Core.Room;

public class Room
{
    public int Id { get; set; }
    public RoomType Type { get; set; }
    
    
    
}

public enum RoomType
{
    StockRoom,
    OperationRoom,
    ExaminationRoom,
    WaitRoom,
    WaitingRoom
}