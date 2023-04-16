using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Exceptions;

namespace ZdravoCorp.Core.Repositories.User;

public class UserRepository
{
    private readonly string _fileName = @".\..\..\..\Data\users.json";
    private readonly List<Models.User.User> _users;
    private readonly JsonSerializerOptions  _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    
    public UserRepository(List<Models.User.User> us)
    {
        this._users = us;
        //this.LoadFromFile();
    }

    public UserRepository()
    {
        _users = new List<Models.User.User>();
        LoadFromFile();
    }

    public void AddUser(Models.User.User user)
    {
        _users.Add(user);
    }
    
    private List<dynamic> ReduceForSerialization()
    {
        return this._users.Select(user => user.GetUserForSerialization()).ToList();
    }
    public void LoadFromFile()
    {
        string text = File.ReadAllText(_fileName);
        if (text == "")
        {
            throw new EmptyFileException("File is empty!");
        }

        try
        {

            var users = JsonSerializer.Deserialize<List<Models.User.User>>(text, _serializerOptions);

            users?.ForEach(user => _users.Add(user));
        }
        catch (JsonException e)
        {
            Trace.WriteLine(e);
            throw;
        }
    }

    public void SaveToFile()
    {
        if (_users.Count == 0)
        {
            Trace.WriteLine($"Repository is empty! {this.GetType()}");
            return;
        }
        var usersForFile = ReduceForSerialization();
        var users = JsonSerializer.Serialize(usersForFile, _serializerOptions);
        File.WriteAllText(this._fileName, users);
    }

    public Models.User.User? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(user => user.Email == email);
    }

    public bool ValidateEmail(string email)
    {
        return _users.Exists(user => user.Email == email);
    }


    
    
    
    
}