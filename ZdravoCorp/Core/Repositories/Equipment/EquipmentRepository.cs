using System.Collections.Generic;

namespace ZdravoCorp.Core.Repositories.Equipment;

public class EquipmentRepository
{
    private List<Models.Equipment.Equipment> equipments;
    private const string _fileName = "";

    public void SaveToFile()
    {
        
        
    }

    public void Add(Models.Equipment.Equipment newEquipment)
    {
        equipments.Add(newEquipment);
    }
}