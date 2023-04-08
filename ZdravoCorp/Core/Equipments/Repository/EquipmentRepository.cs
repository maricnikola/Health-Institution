using System.Collections.Generic;

namespace ZdravoCorp.Core.Equipments.Repository;

public class EquipmentRepository
{
    private List<Model.Equipment> equipments;
    private const string _fileName = "";

    public void SaveToFile()
    {
        
        
    }

    public void Add(Model.Equipment newEquipment)
    {
        equipments.Add(newEquipment);
    }
}