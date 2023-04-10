using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ZdravoCorp.Core.Loader;

namespace ZdravoCorp.View;

public partial class LoginDialog : Window, INotifyPropertyChanged
{
    private string? _email;
    private string? _password;
    public string Email
    {
        get
        {
            return _email;
        }
        set
        {
            if (value != _email)
            {
                _email = value;
                OnPropertyChanged("EmailBox");
            }
        }
    }

    public string Password
    {
        get
        {
            return _password;
        }
        set
        {
            if (value != _password)
            {
                _password = value;
                OnPropertyChanged("PasswordBox");
            }
        }
    }
    public LoginDialog()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("email: " + Email + "   password: " + Password, "Test", MessageBoxButton.OK);
        
        
        
        /*DialogResult = true;
        Close();*/
    }



    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        Password = PasswordBox.Password;
    }
}