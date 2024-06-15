using System.Windows;

namespace ZdravoCorp.GUI.PatientFiles.MedicalRecords.Views;

/// <summary>
///     Interaction logic for MedicalRecordView.xaml
/// </summary>
public partial class MedicalRecordView : Window
{
    public MedicalRecordView()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}