using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.User;

public class UserRepository : ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\users.json";
    private  List<Models.Users.User> _users;

    
    public UserRepository(List<Models.Users.User> us)
    {
        this._users = us;
        //this.LoadFromFile();
    }

    public UserRepository()
    {
        _users = new List<Models.Users.User>();
        Serializer.Load(this);
    }

    public void AddUser(Models.Users.User user)
    {
        _users.Add(user);
    }
    





    public Models.Users.User? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(user => user.Email == email);
    }

    public bool ValidateEmail(string email)
    {
        return _users.Exists(user => user.Email == email);
    }


    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _users;
    }

    public void Import(JToken token)
    {
        _users = token.ToObject<List<Models.Users.User>>();
    }
}