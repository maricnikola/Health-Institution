using System.Collections.Generic;
using ZdravoCorp.Core.HospitalAssets.Equipment.Models;

namespace ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;

public interface IEquipmentService
{
    public List<Equipment.Models.Equipment>? GetAll();
    public Equipment.Models.Equipment? GetById(int id);
    public void AddEquipment(EquipmentDTO equipmentDto);
    public void Update(int id, EquipmentDTO equipmentDto);
    public void Delete(int id);





}