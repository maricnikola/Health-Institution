using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ZdravoCorp.Core.Repositories.User;

public class UserRepository
{
    private readonly string _fileName = @".\..\..\..\Data\users.json";
    private List<Models.User.User> _users;
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
        var users = JsonSerializer.Deserialize<List<Models.User.User>>(text, _serializerOptions);

        users.ForEach(user => _users.Add(user));
    }

    public void SaveToFile()
    {
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