using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ZdravoCorp.Core.Loader;
using ZdravoCorp.Core.User;
using ZdravoCorp.Core.User.Repository;

namespace ZdravoCorp.View;

public partial class LoginDialog : Window, INotifyPropertyChanged
{
    private string? _email;
    private string? _password;
    private UserRepository _userRepository;
    private DoctorRepository _doctorRepository;

    
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
    public LoginDialog(UserRepository userRepository, DoctorRepository doctorRepository)
    {
        _userRepository = userRepository;
        _doctorRepository = doctorRepository;
        InitializeComponent();
        DataContext = this;
    }

    private void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("email: " + Email + "   password: " + Password, "Test", MessageBoxButton.OK);
        var user = GetLoggedUser();
        if (user==null)
            return;
        DialogResult = true;

 
             
        switch (user.Type)
        {
                case User.UserType.Director:
                    //start director view
                    MessageBox.Show("Director", "UserType", MessageBoxButton.OK);

                    Close();
                    break;
                case User.UserType.Patient:
                    //start patient view
                    MessageBox.Show("Patient", "UserType", MessageBoxButton.OK);
                    Close();
                    break;
                case User.UserType.Nurse:
                    //start nurse view
                    MessageBox.Show("Nurse", "UserType", MessageBoxButton.OK);
                    Close();
                    break;
                case User.UserType.Doctor:
                    //start doctor view
                    MessageBox.Show("Doctor", "UserType", MessageBoxButton.OK);
                    Close();
                    var doctorWindow = new DoctorFrame(user,_doctorRepository);
                    doctorWindow.Show();
                    break;

        }
        
        
    }


    private User? GetLoggedUser()
    {
        if (!_userRepository.ValidateEmail(Email))
        {
            MessageBox.Show("Invalid Email", "Error", MessageBoxButton.OK);
            return null;
        }
        else
        {
            var user = _userRepository.GetUserByEmail(Email);
            if (user != null && user.ValidatePassword(Password))
            {
                return user;
            }
            MessageBox.Show("Invalid Password", "Error", MessageBoxButton.OK);
            return null;
        }
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