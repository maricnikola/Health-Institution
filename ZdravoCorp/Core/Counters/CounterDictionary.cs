using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ZdravoCorp.Core.Counters;


public class CounterDictionary
{
    private readonly string _fileName = @".\..\..\..\Data\counters.json";
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        //WriteIndented = true
        PropertyNameCaseInsensitive = true
    };
    public Dictionary<string, Counter>? AllCounters;

    public CounterDictionary()
    {
        AllCounters = new Dictionary<string, Counter>();
        LoadFromFile();
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
        {
            Counter c = new Counter();
            c.Cancelations = new List<DateTime> { date };
            AllCounters.Add(email, c);
        }
            
            //AllCounters[email].Cancelations =
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
        try
        {
            return AllCounters[email].Cancelations.Count >= 5 || AllCounters[email].News.Count>=8;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public void LoadFromFile()
    {
        string text = File.ReadAllText(_fileName);
        if (text == "")
        {
            return;
        }
        Dictionary<string, Counter>? users = JsonConvert.DeserializeObject<Dictionary<string, Counter>>(text);

        AllCounters = users;
    }

    public void SaveToFile()
    {
        var counters = JsonConvert.SerializeObject(AllCounters, Formatting.Indented);
        File.WriteAllText(this._fileName, counters);
    }

}