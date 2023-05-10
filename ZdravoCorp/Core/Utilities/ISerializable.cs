using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.User;

namespace ZdravoCorp.Core.Utilities;

public interface ISerializable
{
    public string FileName();
    
    public IEnumerable<object>? GetList();
    public void Import(JToken token);
}