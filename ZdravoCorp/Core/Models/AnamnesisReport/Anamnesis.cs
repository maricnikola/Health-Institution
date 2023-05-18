using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Models.AnamnesisReport;

public class Anamnesis
{
    [JsonConstructor]
    public Anamnesis(List<string> symptoms, string opinion, string keyWord, List<string> alerrgens)
    {
        Symptoms = symptoms;
        Opinion = opinion;
        KeyWord = keyWord;
        Allergens = alerrgens;
    }

    public List<string> Symptoms { get; set; }
    public string Opinion { get; set; }
    public string KeyWord { get; set; }
    public List<string> Allergens { get; set; }

    public string SymptomsToString()
    {
        var ssymptoms = "";
        foreach (var symptom in Symptoms) ssymptoms += symptom + " ; ";
        return ssymptoms;
    }   
    
    public string AllergensToString()
    {
        var sallergens = "";
        foreach (var allergen in Allergens) sallergens += allergen + " ; ";
        return sallergens;
    }

    public Anamnesis(AnamnesisDTO anamnesisDto)
    {
        Symptoms = anamnesisDto.Symptoms;
        Allergens = anamnesisDto.Allergens;
        Opinion = anamnesisDto.Opinion;
        KeyWord = anamnesisDto.KeyWord;
    }
}