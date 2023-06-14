using System;
using System.Collections.Generic;
using ZdravoCorp.Core.HospitalAssets.Rooms.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalAssets.Rooms.Services;

public interface IRenovationService
{
    public List<Renovation>? GetAll();
    public Renovation? GetById(int id);
    public void AddRenovation(RenovationDTO renovationDto);
    public void UpdateStatus(int id, Renovation.RenovationStatus status);

    public void Update(int id, RenovationDTO renovationDto);

    public void Delete(int id);
    public bool IsRenovationScheduled(int roomId, TimeSlot slot);
    public bool HasRenovationsAfter(int roomId, DateTime start);
    
    public event EventHandler DataChanged;
}