using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Models.Renovation;
using ZdravoCorp.Core.Repositories.RenovationRepo;
using ZdravoCorp.Core.Services.RoomServices;

namespace ZdravoCorp.Core.Services.RenovationServices;

public class RenovationService : IRenovationService

{
    private IRenovationRepository _renovationRepository;

    public RenovationService(IRenovationRepository renovationRepository)
    {
        _renovationRepository = renovationRepository;
    }
    public List<Renovation>? GetAll()
    {
        return _renovationRepository.GetAll() as List<Renovation>;
    }

    public Renovation? GetById(int id)
    {
        return _renovationRepository.GetById(id);
    }

    public void AddRenovation(RenovationDTO renovationDto)
    {
        _renovationRepository.Insert(new Renovation(renovationDto));
        DataChanged?.Invoke(this, new EventArgs());
    }

    public void UpdateStatus(int id, Renovation.RenovationStatus status)
    {
       _renovationRepository.UpdateStatus(id, status);
       DataChanged?.Invoke(this, new EventArgs());
    }

    public void Update(int id, RenovationDTO renovationDto)
    {
        var oldRenovation = _renovationRepository.GetById(id);
        if (oldRenovation == null)
        {
            throw new KeyNotFoundException();
        }
        _renovationRepository.Delete(oldRenovation);
        _renovationRepository.Insert(new Renovation(renovationDto));
        DataChanged?.Invoke(this, new EventArgs());
    }

    public void Delete(int id)
    {
        _renovationRepository.Delete(GetById(id));
        DataChanged?.Invoke(this, new EventArgs());
    }



    public event EventHandler? DataChanged;
}