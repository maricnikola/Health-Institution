using System.Windows;
using ZdravoCorp.Core.Loader;

namespace ZdravoCorp.View;

public partial class LoginDialog : Window
{
    public LoginDialog()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        LoadFunctions.LoadUsers();


        Close();
    }
}