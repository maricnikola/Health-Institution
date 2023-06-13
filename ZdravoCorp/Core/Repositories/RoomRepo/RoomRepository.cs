using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.RoomRepo;

public class RoomRepository : ISerializable, IRoomRepository
{
    private readonly string _fileName = @".\..\..\..\..\Data\rooms.json";


    public RoomRepository()
    {
        _rooms = new List<Room>();
        Serializer.Load(this);
    }

    private List<Room>? _rooms;

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
        _rooms = token.ToObject<List<Room>>();
    }

    public void Insert(Room newRoom)
    {
        _rooms.Add(newRoom);
        Serializer.Save(this);
    }




    public IEnumerable<Room> GetAll()
    {
        return _rooms;
    }



    public void Delete(Room entity)
    {
        _rooms.Remove(entity);
        Serializer.Save(this);
    }

    public Room? GetById(int id)
    {
        return _rooms.FirstOrDefault(room => room.Id == id);
    }


}