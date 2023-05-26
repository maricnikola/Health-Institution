﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Models.Presriptions;

public class Prescription
{
    public string Medicament { get; set; }
    public int TimesADay { get; set; }
    public string Instructions { get; set; }
    public DateTime ExpirationDate { get; set; }
    public List<int> HourlyRates { get; set; }

    [JsonConstructor]
    public Prescription(string medicament, int timesADay, string instructions, DateTime expirationDate, List<int> hourlyRates)
    {
        Medicament = medicament;
        TimesADay = timesADay;
        Instructions = instructions;
        ExpirationDate = expirationDate;
        HourlyRates = hourlyRates;
    }
}
