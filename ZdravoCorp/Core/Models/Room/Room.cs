using System.Text.Json.Serialization;

namespace ZdravoCorp.Core.Models.Room;

public class Room
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RoomType Type { get; set; }

    [JsonConstructor]
    public Room(int id, RoomType type)
    {
        Id = id;
        Type = type;
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