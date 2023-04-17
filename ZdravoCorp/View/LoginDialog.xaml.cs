﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ZdravoCorp.Core.Loader;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels;
using ZdravoCorp.Core.ViewModels.Director;
using ZdravoCorp.View.Director;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.View;

public partial class LoginDialog : Window, INotifyPropertyChanged
{
    private string? _email;
    private string? _password;
    private UserRepository _userRepository;
    private DoctorRepository _doctorRepository;
    private InventoryRepository _inventoryRepository;
    
    private PatientRepository _patientRepository;
    private ScheduleRepository _scheduleRepository;
    
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
    public LoginDialog(UserRepository userRepository, PatientRepository patientRepository ,DoctorRepository doctorRepository, ScheduleRepository scheduleRepository,InventoryRepository inventoryRepository)
    {
        _patientRepository = patientRepository;
        _userRepository = userRepository;
        _doctorRepository = doctorRepository;
        _inventoryRepository = inventoryRepository;
        _scheduleRepository = scheduleRepository;
        InitializeComponent();
        DataContext = this;
    }

    private void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        var user = GetLoggedUser();
        if (user==null)
            return;
        DialogResult = true;

 
             
        switch (user.Type)
        {
                case User.UserType.Director:
                    //start director view
                    Application.Current.MainWindow = new DirectorWindow() {DataContext = new DirectorViewModel(_inventoryRepository)};;
                    break;
                case User.UserType.Patient:
                    //start patient view
                    Application.Current.MainWindow = new PatientFrame(user, _patientRepository, _doctorRepository, _scheduleRepository);
                    break;
                case User.UserType.Nurse:
                    //start nurse view
                    //Application.Current.MainWindow = new NurseWindow(){DataContext = new NurseViewModel()};;
                    break;
                case User.UserType.Doctor:
                //start doctor view
                    Application.Current.MainWindow = new AppointmentsShowView() { DataContext = new AppointmentShowViewModel(user,_scheduleRepository,_doctorRepository,_patientRepository)};
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