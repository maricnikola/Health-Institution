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
    [JsonPropertyName("IsDynamic")]
    public bool IsDynamic { get; set; }


    [JsonConstructor]
    public Equipment(int id, string name, EquipmentType type, bool isDynamic)
    {
        Id = id;
        Name = name;
        Type = type;
        IsDynamic = isDynamic;
    }
    
    
    public enum EquipmentType
    {
        Operation,
        Examination,
        Room,
        Hallway
    }

    protected bool Equals(Equipment other)
    {
        return Id == other.Id && Name == other.Name && Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Equipment)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, (int)Type);
    }
}

