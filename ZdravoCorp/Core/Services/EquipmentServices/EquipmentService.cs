using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Models.Equipments;
using ZdravoCorp.Core.Repositories.EquipmentRepo;

namespace ZdravoCorp.Core.Services.EquipmentServices;

public class EquipmentService : IEquipmentService
{
    private IEquipmentRepository _equipmentRepository;

    public EquipmentService(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public List<Equipment>? GetAll()
    {
        return _equipmentRepository.GetAll() as List<Equipment>;
    }

    public Equipment? GetById(int id)
    {
        return _equipmentRepository.GetById(id);
    }

    public void AddEquipment(EquipmentDTO equipmentDto)
    {
        _equipmentRepository.Insert(new Equipment(equipmentDto));
    }

    public void Update(int id, EquipmentDTO equipmentDto)
    {
        var oldEquipment = _equipmentRepository.GetById(id);
        if (oldEquipment == null)
            throw new KeyNotFoundException();
        _equipmentRepository.Delete(oldEquipment);
        _equipmentRepository.Insert(new Equipment(equipmentDto));
    }

    

    public void Delete(int id)
    {
        _equipmentRepository.Delete(_equipmentRepository.GetById(id));
    }
    
}