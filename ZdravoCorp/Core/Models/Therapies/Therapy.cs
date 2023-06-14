using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Models.Therapies;

public class Therapy
{
    public string Description { get; set; }
    public bool Status { get; set; }

    public Therapy(string description, bool status)
    {
        Description = description;
        Status = status;
    }
}
