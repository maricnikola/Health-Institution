using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Documents;

namespace ZdravoCorp.Core.User.Repository;

public class UserRepository
{
    private String _fileName = @"..\\users.json";
    public List<User> Users;
    

    private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {

    };
    
    public UserRepository(List<User> us)
    {
        this.Users = us;
        //this.LoadFromFile();
    }

    public UserRepository()
    {
        LoadFromFile();
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

    public User? GetUserByEmail(string email)
    {
        return Users.FirstOrDefault(user => user.Email == email);
    }

    public bool ValidateEmail(string email)
    {
        return Users.Exists(user => user.Email == email);
    }

    public bool ValidatePassword(string email, string password)
    {
        
        //TODO add validation for password and email 
        return true;
    }
    
    
    
    
}