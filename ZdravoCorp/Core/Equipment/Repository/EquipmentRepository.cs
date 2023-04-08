using System.Collections.Generic;

namespace ZdravoCorp.Core.Equipment.Repository;

public class EquipmentRepository
{
    private List<Model.Equipment> Equipments;
    private const string _fileName = "";

    public void SaveToFile()
    {
        
        
    }

    public void Add(Model.Equipment newEquipment)
    {
        Equipments.Add(newEquipment);
    }
}