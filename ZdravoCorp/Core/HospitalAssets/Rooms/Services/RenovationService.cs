using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.HospitalAssets.Rooms.Models;
using ZdravoCorp.Core.HospitalAssets.Rooms.Repositories;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalAssets.Rooms.Services;

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

    public bool IsRenovationScheduled(int roomId, TimeSlot slot)
    {
        foreach (var renovation in _renovationRepository.GetAll()
                     .Where(ren => ren.Room.Id == roomId || (ren.Join != null && ren.Join.Id == roomId)))
        {
            if (renovation.Status != Renovation.RenovationStatus.Finished && renovation.Slot.Overlap(slot))
                return true;
        }

        return false;
    }

    public bool HasRenovationsAfter(int roomId, DateTime start)
    {
        foreach (var renovation in _renovationRepository.GetAll()
                     .Where(ren => ren.Room.Id == roomId || (ren.Join != null && ren.Join.Id == roomId)))
        {
            if (renovation.Status != Renovation.RenovationStatus.Finished && renovation.Slot.Start > start)
                return true;
        }

        return false;
    }


    public event EventHandler? DataChanged;
}