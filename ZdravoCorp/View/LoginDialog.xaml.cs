using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ZdravoCorp.Core.Counters;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.Order;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels;
using ZdravoCorp.Core.ViewModels.DirectorViewModel;
using ZdravoCorp.Core.ViewModels.PatientViewModel;
using ZdravoCorp.View.Director;
using ZdravoCorp.View.Director;
using ZdravoCorp.View.PatientV;
using ZdravoCorp.View.DoctorView;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.ViewModels.DoctorViewModels;

namespace ZdravoCorp.View;

public partial class LoginDialog : Window, INotifyPropertyChanged
{
    private string? _email;
    private string? _password;
    private readonly UserRepository _userRepository;
    private readonly DoctorRepository _doctorRepository;
    private readonly InventoryRepository _inventoryRepository;
    private readonly PatientRepository _patientRepository;
    private readonly ScheduleRepository _scheduleRepository;
    private readonly OrderRepository _orderRepository;
    private readonly MedicalRecordRepository _medicalRecordRepository;
    
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
    public LoginDialog(UserRepository userRepository, PatientRepository patientRepository ,DoctorRepository doctorRepository, ScheduleRepository scheduleRepository,InventoryRepository inventoryRepository, OrderRepository orderRepository, MedicalRecordRepository medicalRecordRepository)
    {
        _patientRepository = patientRepository;
        _userRepository = userRepository;
        _doctorRepository = doctorRepository;
        _scheduleRepository = scheduleRepository;
        _inventoryRepository = inventoryRepository;
        _orderRepository = orderRepository;
        _medicalRecordRepository = medicalRecordRepository;
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
                    Application.Current.MainWindow = new DirectorWindow() {DataContext = new DirectorViewModel(_inventoryRepository, _orderRepository)};;
                    break;
                case User.UserType.Patient:
                    //start patient view
                    User.State state = user.UserState;
                    Patient patient = _patientRepository.GetPatientByEmail(user.Email);
                    List<Appointment> appointments = _scheduleRepository.GetPatientAppointments(patient.Email);
                    CounterDictionary counterDictionary = new CounterDictionary();
                    if (counterDictionary.IsForBlock(user.Email))
                    {
                        
                        MessageBox.Show("You are blocked", "Error", MessageBoxButton.OK);
                        DialogResult = false;
                    }
                    else
                        Application.Current.MainWindow = new PatientWindow()
                        {
                            DataContext = new PatientViewModel(appointments, _scheduleRepository, _doctorRepository,
                                patient, _medicalRecordRepository)
                        };

                    break;
                case User.UserType.Nurse:
                    //start nurse view
                    //Application.Current.MainWindow = new NurseWindow(){DataContext = new NurseViewModel()};;
                    break;
                case User.UserType.Doctor:
                //start doctor view
                    Application.Current.MainWindow = new DoctorWindow() { DataContext = new DoctorViewModel(user,_scheduleRepository, _doctorRepository, _patientRepository,_medicalRecordRepository) };
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