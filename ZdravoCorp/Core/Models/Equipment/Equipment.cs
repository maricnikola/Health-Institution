using System;

namespace ZdravoCorp.Core.Models.Equipment;

public class Equipment
{
    public String Name { get; set; }
    public EquipmentType Type { get; set; }
    public int Quantity { get; set; }
}

public enum EquipmentType
{
    Operation,
    Appointment,
    Room,
    Hallway
}