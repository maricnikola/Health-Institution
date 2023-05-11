using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Models.AnamnesisReport;

public class Anamnesis
{
    public List<String> Symptoms { get; set; }
    public String Opinion { get; set; }
    public String KeyWord { get; set; }
    public List<String> Allergens { get; set; }

    public Anamnesis(List<String> symptoms, String opinion, String keyWord, List<String> alerrgens)
    {
        Symptoms = symptoms;
        Opinion = opinion;
        KeyWord = keyWord;
        Allergens = alerrgens;
    }

    public string SymptomsToString()
    {
        string ssymptoms = "";
        foreach (var symptom in Symptoms)
        {
            ssymptoms += symptom + " ; ";
        }
        return ssymptoms;
    }

    public string AllergensToString()
    {
        string sallergens= "";
        foreach (var allergen in Allergens)
        {
            sallergens += allergen + " ; ";
        }
        return sallergens;
    }


}
