using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.Room;

public class RoomRepository : ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\rooms.json";
    private List<Models.Rooms.Room>? _rooms;
    public List<Models.Rooms.Room>? Rooms => _rooms;


    public RoomRepository()
    {
        _rooms = new List<Models.Rooms.Room>();
        Serializer.Load(this);
    }

    public void Add(Models.Rooms.Room newRoom)
    {
        _rooms.Add(newRoom);
    }

    public IEnumerable<Models.Rooms.Room> GetAllExcept(int roomId)
    {
        return _rooms.Where(room => room.Id != roomId);
    }


    public Models.Rooms.Room? GetById(int id)
    {
        return _rooms.FirstOrDefault(room => room.Id == id);
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _rooms;
    }

    public void Import(JToken token)
    {
        _rooms = token.ToObject<List<Models.Rooms.Room>>();
    }
}