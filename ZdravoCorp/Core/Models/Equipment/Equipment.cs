using System;
using System.Text.Json.Serialization;

namespace ZdravoCorp.Core.Models.Equipment;

public class Equipment
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EquipmentType Type { get; set; }
    //public int Quantity { get; set; }


    [JsonConstructor]
    public Equipment(int id, string name, EquipmentType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }
    
    
    public enum EquipmentType
    {
        Operation,
        Examination,
        Room,
        Hallway
    }
}

