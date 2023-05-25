using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Models.Medicaments;

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
