using System.Collections.Generic;
using ZdravoCorp.Core.User.Repository;

namespace ZdravoCorp.Core.Loader;
using ZdravoCorp.Core.User;
public class LoadFunctions
{
    public static void LoadUsers()
    {
        User u1 = new User("123", 12, "miso", "Miso", "Misic");
        User u2 = new User("123", 12, "miso1", "Miso1", "Misic1");
        User u3 = new User("123", 12, "miso2", "Miso2", "Misic2");
        User u4 = new User("123", 12, "miso3", "Miso3", "MisIc3");

        List<User> users = new List<User>();
        users.Add(u1);
        users.Add(u2);
        users.Add(u3);
        users.Add(u4);

        UserRepository repository = new UserRepository(users);
        repository.SaveToFile();
        
        

    }
}