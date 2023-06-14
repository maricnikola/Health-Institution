using System;
using System.Collections.Generic;

namespace ZdravoCorp.Core.PatientFiles.Presriptions.Models;

public class Medicament
{
    public string Name { get; set; }
    public List<String> Components { get; set; }

    public Medicament(string name, List<string> components)
    {
        Name = name;
        Components = components;
    }
}
