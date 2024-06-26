﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Autofac;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.Counters;
using ZdravoCorp.GUI.Main.Director;
using ZdravoCorp.GUI.Main.Doctor;
using ZdravoCorp.GUI.Main.Nurse;
using ZdravoCorp.GUI.Main.Patient;

namespace ZdravoCorp.GUI.Main;

public partial class LoginDialog : Window, INotifyPropertyChanged
{
    private string? _email;
    private string? _password;
    private IUserService _userService;

    public LoginDialog()
    {
        InitializeComponent();
        _userService = Injector.Container.Resolve<IUserService>();
        DataContext = this;
    }

    public string Email
    {
        get => _email;
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
        get => _password;
        set
        {
            if (value != _password)
            {
                _password = value;
                OnPropertyChanged("PasswordBox");
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        var user = GetLoggedUser();
        if (user == null)
            return;
        DialogResult = true;


        switch (user.Type)
        {
            case User.UserType.Director:
                //Start director view
                Application.Current.MainWindow = new DirectorWindow
                    { DataContext = new DirectorViewModel() };
                ;
                break;
            case User.UserType.Patient:
                //Start patient view
                var state = user.UserState;
                var counterDictionary = new CounterDictionary();
                if (counterDictionary.IsForBlock(user.Email))
                {
                    MessageBox.Show("You are blocked", "Error", MessageBoxButton.OK);
                    DialogResult = false;
                }
                else
                {
                    Application.Current.MainWindow = new PatientWindow
                    {
                        DataContext = new PatientViewModel(user)
                    };
                }

                break;
            case User.UserType.Nurse:
                //Start nurse view
                Application.Current.MainWindow = new NurseWindow
                    { DataContext = new NurseViewModel(user) };
                break;
            case User.UserType.Doctor:
                //Start doctor view
                Application.Current.MainWindow = new DoctorWindow
                    { DataContext = new DoctorViewModel(user) };
                break;
        }
    }

    private User? GetLoggedUser()
    {
        if (!_userService.ValidateEmail(Email))
        {
            MessageBox.Show("Invalid Email", "Error", MessageBoxButton.OK);
            return null;
        }

        var user = _userService.GetByEmail(Email);
        if (user != null && user.ValidatePassword(Password)) return user;
        MessageBox.Show("Invalid Password", "Error", MessageBoxButton.OK);
        return null;
    }

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