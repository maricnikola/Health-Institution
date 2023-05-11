using ZdravoCorp.Core.Models.Rooms;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class RoomViewModel : ViewModelBase
{
    private Room _room;
    public int Id => _room.Id;
    public string Type => _room.Type.ToString();


    public RoomViewModel(Room room)
    {
        _room = room;
    }
}