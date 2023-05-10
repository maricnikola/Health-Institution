using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.Room;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.Room;

public class RoomRepository : ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\rooms.json";
    private  List<Models.Room.Room>? _rooms;
    

    public RoomRepository()
    {
        _rooms = new List<Models.Room.Room>();
        Serializer.Load(this);
    }
    
    public void Add(Models.Room.Room newRoom)
    {
        _rooms.Add(newRoom);
    }


    public Models.Room.Room? GetById(int id)
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
        _rooms = token.ToObject<List<Models.Room.Room>>();
    }
}