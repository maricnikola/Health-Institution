using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.Room;

namespace ZdravoCorp.Core.Repositories.Room;

public class RoomRepository
{
    private readonly string _fileName = @".\..\..\..\Data\rooms.json";
    private readonly List<Models.Room.Room> _rooms;

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
            throw new EmptyFileException("File is empty!");
        try
        {

            var rooms = JsonSerializer.Deserialize<List<Models.Room.Room>>(text);
            rooms?.ForEach(room => _rooms.Add(room));
        }
        catch (JsonException e)
        {
            Trace.WriteLine(e);
        }
    }
    
    public void SaveToFile()
    {
        if (_rooms.Count == 0)
        {
            Trace.WriteLine($"Repository is empty! {this.GetType()}");
            return;
        }
        var rooms= JsonSerializer.Serialize(_rooms, _serializerOptions);
        File.WriteAllText(this._fileName, rooms);
    }

    public Models.Room.Room? GetById(int id)
    {
        return _rooms.FirstOrDefault(room => room.Id == id);
    }
}