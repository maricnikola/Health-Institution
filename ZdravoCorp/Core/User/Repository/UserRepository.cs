using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Documents;

namespace ZdravoCorp.Core.User.Repository;

public class UserRepository
{
    private readonly string _fileName = @".\..\..\..\Data\users.json";
    private List<User> _users;
    private readonly JsonSerializerOptions  _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    
    public UserRepository(List<User> us)
    {
        this._users = us;
        //this.LoadFromFile();
    }

    public UserRepository()
    {
        _users = new List<User>();
        LoadFromFile();
    }

    public void AddUser(User user)
    {
        _users.Add(user);
    }
    
    private List<dynamic> AdjustForSerialization()
    {
        List<dynamic> usersForFile = new List<dynamic>();
        foreach (User user in this._users)
        {
            usersForFile.Add(user.GetUserForSerialization());
        }
        return usersForFile;
    }
    public void LoadFromFile()
    {
        string text = File.ReadAllText(_fileName);
        var users = JsonSerializer.Deserialize<List<User>>(text, _serializerOptions);

        users.ForEach(user => _users.Add(user));
    }

    public void SaveToFile()
    {
        var usersForFile = AdjustForSerialization();
        var users = JsonSerializer.Serialize(usersForFile, _serializerOptions);
        File.WriteAllText(this._fileName, users);
    }

    public User? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(user => user.Email == email);
    }

    public bool ValidateEmail(string email)
    {
        return _users.Exists(user => user.Email == email);
    }


    
    
    
    
}