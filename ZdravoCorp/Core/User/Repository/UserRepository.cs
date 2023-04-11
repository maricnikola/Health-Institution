using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Documents;

namespace ZdravoCorp.Core.User.Repository;

public class UserRepository
{
    //private String _fileName = "C:\\Users\\Aleksa\\Desktop\\usi-2023-group-3-team-11\\ZdravoCorp\\Core\\User\\users.json";
    private String _fileName = "C:\\Users\\danilo.c\\RiderProjects\\usi-2023-group-3-team-11\\ZdravoCorp\\Core\\User\\users.json";
    
    public List<User> Users;
    
    private Dictionary<string, string> Passwords;

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
        Users = new List<User>();
        //Passwords = new Dictionary<string, string>();
        
        //LoadFromFile();
       // LoadPasswordsFromFile();
    }

    public void AddUser(User user)
    {
        Users.Add(user);
    }
    
    
    // TODO change name and make serialization and deserialization work
    private List<dynamic> AdjustForSerialization()
    {
        List<dynamic> usersForFile = new List<dynamic>();
        foreach (User user in this.Users)
        {
            usersForFile.Add(user.GetUserForSerialization());
        }
        return usersForFile;
    }
    public void LoadFromFile()
    {
        string text = File.ReadAllText(_fileName);
        var users = JsonSerializer.Deserialize<List<User>>(text);

        users.ForEach(user => Users.Add(user));
    }

    public void SaveToFile()
    {
        var usersForFile = AdjustForSerialization();
        var users = JsonSerializer.Serialize(usersForFile, _serializerOptions);
        File.WriteAllText(this._fileName, users);
    }

    /*public void LoadPasswordsFromFile()
    {
        string text = File.ReadAllText(_passwordsFileName);
        var passwords = JsonSerializer.Deserialize<Dictionary<string, string>>(text);
        Passwords = new Dictionary<string, string>(passwords);
    }*/

    public User? GetUserByEmail(string email)
    {
        return Users.FirstOrDefault(user => user.Email == email);
    }

    public bool ValidateEmail(string email)
    {
        return Users.Exists(user => user.Email == email);
    }


    
    
    
    
}