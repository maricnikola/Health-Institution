using ZdravoCorp.Core.Models.Rooms;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class RoomViewModel : ViewModelBase
{
    private readonly Room _room;


    public RoomViewModel(Room room)
    {
        _room = room;
    }

    public int Id => _room.Id;
    public string Type => _room.Type.ToString();

    public string IsUnderRenovation => _room.IsUnderRenovation.ToString();

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Type)}: {Type}, {nameof(IsUnderRenovation)}: {IsUnderRenovation}";
    }
}