using System.Windows;

namespace ZdravoCorp.View;

public partial class LoginDialog : Window
{
    public LoginDialog()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
        this.Close();
    }
}