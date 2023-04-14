using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Models.Room;

namespace ZdravoCorp.Core.Repositories.Room;

public class RoomRepository
{
    private readonly string _fileName = @".\..\..\..\Data\rooms.json";
    private List<Models.Room.Room> _rooms { get; set; }

    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public RoomRepository()
    {
        _rooms = new List<Models.Room.Room>();
        LoadFromFile();
    }
    
    public void Add(Models.Room.Room newRoom)
    {
        _rooms.Add(newRoom);
    }
    public void LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        if (text == "")
            return;
        var rooms = JsonSerializer.Deserialize<List<Models.Room.Room>>(text);
        rooms.ForEach(room => _rooms.Add(room));
    }
    
    public void SaveToFile()
    {
       
        var rooms= JsonSerializer.Serialize(_rooms, _serializerOptions);
        File.WriteAllText(this._fileName, rooms);
    }

    public Models.Room.Room? GetById(int id)
    {
        return _rooms.FirstOrDefault(room => room.Id == id);
    }
}