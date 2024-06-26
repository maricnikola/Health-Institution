﻿using ZdravoCorp.Core.Scheduling.Models;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.Scheduling.ViewModels;

public class FullAnamnesisViewModel : ViewModelBase
{
    private readonly Anamnesis _anamnesis;

    public FullAnamnesisViewModel(Anamnesis anamnesis)
    {
        _anamnesis = anamnesis;
        if (anamnesis == null)
        {
            KeyWord = "";
            Symptoms = "";
            Opinion = "";
            Allergens = "";
        }
        else
        {
            KeyWord = anamnesis.KeyWord;
            Symptoms = anamnesis.SymptomsToString();
            Allergens = anamnesis.AllergensToString();
            Opinion = anamnesis.Opinion;
        }
    }

    public string KeyWord { get; set; }
    public string Opinion { get; set; }
    public string Symptoms { get; set; }
    public string Allergens { get; set; }
}