using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Models.Renovation;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Services.RenovationServices;

public interface IRenovationService
{
    public List<Renovation>? GetAll();
    public Renovation? GetById(int id);
    public void AddRenovation(RenovationDTO renovationDto);
    public void UpdateStatus(int id, Renovation.RenovationStatus status);

    public void Update(int id, RenovationDTO renovationDto);

    public void Delete(int id);
    public bool IsRenovationScheduled(int roomId, TimeSlot slot);
    
    public event EventHandler DataChanged;
}