using System;
using System.ComponentModel;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.Core.User;
using ZdravoCorp.Core.MedicalRecords.Model;
using ZdravoCorp.Core.Rooms;
using System.Security.Policy;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ZdravoCorp.View;


public class Appointment : INotifyPropertyChanged                    
{
    public int Id
    {
        get
        {
            return Id;
        }
        set
        {
            if (value != Id)
            {
                Id = value;
                OnPropertyChanged("PasswordBox");
            }
        }
    }
    public TimeSlot Time { get; set; }
    public Doctor Doctor { get; set; }
    public MedicalRecord MedicalRecord { get; set; }
    public String? Anamnesis { get; set; }
    public Room? Room { get; set; }
    public bool IsCanceled;

    [JsonConstructor]
    public Appointment(int id, TimeSlot t, Doctor doctor, MedicalRecord mr)
    {
        Id = id;
        Time = t;
        Doctor = doctor;
        MedicalRecord = mr;
        Anamnesis = null;
        Room = null;
        IsCanceled = false;
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
}