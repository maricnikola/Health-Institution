using System.Collections.Generic;
using ZdravoCorp.Core.Models.Equipments;

namespace ZdravoCorp.Core.Services.EquipmentServices;

public interface IEquipmentService
{
    public List<Equipment>? GetAll();
    public Equipment? GetById(int id);
    public void AddEquipment(EquipmentDTO equipmentDto);
    public void Update(int id, EquipmentDTO equipmentDto);
    public void Delete(int id);





}