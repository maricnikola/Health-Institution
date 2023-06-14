using System;
using System.Linq;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;

public class GradesViewModel : ViewModelBase
{
    public GradesViewModel(int[] grades, string type)
    {
        OneStar = grades[0];
        TwoStar = grades[1];
        ThreeStar = grades[2];
        FourStar = grades[3];
        FiveStar = grades[4];
        var count = grades.Sum();
        double sum=0;
        for (var i = 0; i < 5; i++)
        {
            sum += grades[i] * (i + 1);
        }
        Overall = String.Format("{0:0.00}", (sum / count));
        if (Overall == "NaN")
            Overall = "0.00";
        Type = type;
    }
    public string Type { get; set; }
    public int OneStar { get; set; }
    public int TwoStar { get; set; }
    public int ThreeStar { get; set; }
    public int FourStar { get; set; }
    public int FiveStar { get; set; }
    public string Overall { get; set; }
}