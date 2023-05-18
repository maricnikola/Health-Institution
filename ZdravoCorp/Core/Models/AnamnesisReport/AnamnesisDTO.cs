using System.Collections.Generic;

namespace ZdravoCorp.Core.Models.AnamnesisReport;

public class AnamnesisDTO
{
    public List<string> Symptoms { get; set; }
    public string Opinion { get; set; }
    public string KeyWord { get; set; }
    public List<string> Allergens { get; set; }


    public AnamnesisDTO(List<string> symptoms, string opinion, string keyWord, List<string> allergens)
    {
        Symptoms = symptoms;
        Opinion = opinion;
        KeyWord = keyWord;
        Allergens = allergens;
    }
}