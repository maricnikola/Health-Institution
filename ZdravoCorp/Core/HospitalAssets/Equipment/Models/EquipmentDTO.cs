namespace ZdravoCorp.Core.HospitalAssets.Equipment.Models;

public class EquipmentDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Equipment.EquipmentType Type { get; set; }
    public bool IsDynamic { get; set; }

    public EquipmentDTO(int id, string name, Equipment.EquipmentType type, bool isDynamic)
    {
        Id = id;
        Name = name;
        Type = type;
        IsDynamic = isDynamic;
    }
}