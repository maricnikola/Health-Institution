using System;
using ZdravoCorp.Core.HospitalAssets.Rooms.Models;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalAssets.Rooms.ViewModels;

public class RoomViewModel : ViewModelBase
{
    private readonly Room _room;


    public RoomViewModel(Room room)
    {
        _room = room;
    }

    public int Id => _room.Id;
    public string Type => _room.Type.ToString();

    public bool IsUnderRenovation => _room.IsUnderRenovation;

    public override string ToString()
    {
        return String.Format("{0,-5} | {1, -15} | {2, 10}", Id, Type, IsUnderRenovation);
    }
}