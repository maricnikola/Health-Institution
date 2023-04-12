using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ZdravoCorp.Core.MedicalRecords.Model;
using ZdravoCorp.Core.Rooms;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.Core.User;

namespace ZdravoCorp.Core.Appointments.Model
{
    public class AppointmentModel : INotifyPropertyChanged
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
        public TimeSlot Time {
            get
            {
                return Time;
            }
            set
            {
                if (value != Time)
                {
                    Time = value;
                    OnPropertyChanged("PasswordBox");
                }
            }
        }
        public Doctor Doctor { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
        public String? Anamnesis {
            get
            {
                return Anamnesis;
            }
            set
            {
                if (value != Anamnesis)
                {
                    Anamnesis = value;
                    OnPropertyChanged("name");
                }
            }
        }
        public Room? Room { get; set; }
        public bool IsCanceled;

        public AppointmentModel(int id, TimeSlot t, Doctor doctor, MedicalRecord mr)
        {
            Id = id;
            Time = t;
            Doctor = doctor;
            MedicalRecord = mr;
            Anamnesis = null;
            Room = null;
            IsCanceled = false;
        }

        public AppointmentModel(string a)
        {
            Anamnesis = a;
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
}
