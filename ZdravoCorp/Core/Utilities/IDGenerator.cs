using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.Inventory;

namespace ZdravoCorp.Core.Utilities;

public class IDGenerator
{
    private static int _currentId;
    private static readonly string _fileName = @".\..\..\..\Data\idg.json";

    public int CurrentId { get; set; }

    public IDGenerator()
    {
        _currentId = LoadFromFile();
    }

    public static void SaveToFile()
    {
        var id = JsonSerializer.Serialize(_currentId);

        File.WriteAllText(_fileName, id);
    }

    public int LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        if (text == "")
            throw new EmptyFileException("File is empty!");
        try
        {
            var id = JsonSerializer.Deserialize<int>(text);
            return id;
        }
        catch (JsonException e)
        {
            Trace.WriteLine(e);
        }

        return -1;
    }

    public static int GetId()
    {
        _currentId++;
        SaveToFile();
        return _currentId;
    }
}