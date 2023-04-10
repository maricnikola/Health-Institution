using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Documents;

namespace ZdravoCorp.Core.User.Repository;

public class UserRepository
{
    private String _fileName = "C:\\Users\\danilo.c\\RiderProjects\\usi-2023-group-3-team-11\\ZdravoCorp\\Core\\User\\users.json";
    //private String _passwordsFileName = "C:\\Users\\danilo.c\\RiderProjects\\usi-2023-group-3-team-11\\ZdravoCorp\\Core\\User\\passwords.json";
    
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
    private List<dynamic> PrepareForSerialization()
    {
        List<dynamic> reducedDoctors = new List<dynamic>();
        foreach (Doctor doctor in this.Doctors)
        {
            reducedDoctors.Add(user.G);
        }
        return reducedDoctors;
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