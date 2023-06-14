using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using ZdravoCorp.Core.Utilities.Exceptions;
using JsonException = System.Text.Json.JsonException;

namespace ZdravoCorp.Core.Utilities;

public class IDGenerator : IIDGenerator
{
    public static int GetId()
    {
       return BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0);
    }

}