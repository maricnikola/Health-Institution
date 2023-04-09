using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Documents;

namespace ZdravoCorp.Core.User.Repository;

public class UserRepository
{
    private String _fileName = "users.json";
    public List<User> Users;
    
    private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {

    };
    
    public UserRepository(List<User> us)
    {
        this.Users = us;
        this.LoadFromFile();
    }


    public void LoadFromFile()
    {

        string text = File.ReadAllText(_fileName);
        var users = JsonSerializer.Deserialize<List<User>>(text);
        users.ForEach(user => Users.Add(user));
    }

    public void SaveToFile()
    {
        var users = JsonSerializer.Serialize(this.Users, _serializerOptions);
        File.WriteAllText(this._fileName, users);
    }
}