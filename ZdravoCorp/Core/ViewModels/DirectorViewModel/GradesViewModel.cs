namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class GradesViewModel : ViewModelBase
{
    public GradesViewModel(int oneStar, int twoStar, int threeStar, int fourStar, int fiveStar, string overall, string type)
    {
        OneStar = oneStar;
        TwoStar = twoStar;
        ThreeStar = threeStar;
        FourStar = fourStar;
        FiveStar = fiveStar;
        Overall = overall;
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