﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Counters;

public class CounterDictionary
{
    private readonly string _fileName = @".\..\..\..\Data\counters.json";
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    public Dictionary<string, Counter> AllCounters;

    public CounterDictionary()
    {
        AllCounters = new Dictionary<string, Counter>();
        //LoadFromFile();
    }

    public void AddCancelation(string email, DateTime date)
    {
        if (AllCounters.ContainsKey(email))
        {
            AllCounters[email].Cancelations.Add(date);
            foreach (var d in AllCounters[email].Cancelations)
            {
                DateTime monthAgo = DateTime.Now - TimeSpan.FromDays(30);
                if (d < monthAgo)
                    AllCounters[email].Cancelations.Remove(d);
            }
        }
        else
            AllCounters[email].Cancelations = new List<DateTime> { date };
        SaveToFile();
    }
    public void AddNews(string email, DateTime date)
    {
        if (AllCounters.ContainsKey(email))
        {
            AllCounters[email].Cancelations.Add(date);
            foreach (var d in AllCounters[email].Cancelations)
            {
                DateTime monthAgo = DateTime.Now - TimeSpan.FromDays(30);
                if (d < monthAgo)
                    AllCounters[email].Cancelations.Remove(d);
            }
        }

        else
        {
            Counter c = new Counter();
            c.News = new List<DateTime> { date };
            AllCounters.Add(email, c);
        }
        SaveToFile();
    }

    public bool IsForBlock(string email)
    {
        return AllCounters[email].Cancelations.Count >= 5 || AllCounters[email].News.Count>=8;
    }

    public void LoadFromFile()
    {
        string text = File.ReadAllText(_fileName);
        var users = JsonSerializer.Deserialize<Dictionary<string,Counter>>(text, _serializerOptions);

        AllCounters = users;
    }

    public void SaveToFile()
    {
        var users = JsonSerializer.Serialize(AllCounters, _serializerOptions);
        File.WriteAllText(this._fileName, users);
    }


}