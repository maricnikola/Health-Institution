﻿using ZdravoCorp.Core.Models.Renovation;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class RenovationViewModel : ViewModelBase
{
    private readonly Renovation _renovation;

    public RenovationViewModel(Renovation renovation)
    {
        _renovation = renovation;
    }

    public int Room => _renovation.Room.Id;
    public string Start => _renovation.Slot.Start.ToString();
    public string Until => _renovation.Slot.End.ToString();

    public string Split
    {
        get
        {
            if (_renovation.Split == null)
                return " ";
            return _renovation.Split.Id.ToString() + " - "+ _renovation.Split.Type.ToString();
        }
    }
    
    public string Join
    {
        get
        {
            if (_renovation.Join == null)
                return " ";
            return _renovation.Join.Id.ToString() + " - " + _renovation.Join.Type.ToString();
        }
    }
    public string Status => _renovation.Status.ToString();
    
}