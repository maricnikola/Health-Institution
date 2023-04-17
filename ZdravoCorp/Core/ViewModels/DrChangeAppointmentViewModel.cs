using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.TimeSlots;

namespace ZdravoCorp.Core.ViewModels
{
    class DrChangeAppointmentViewModel:ViewModelBase
    {
        private ObservableCollection<String> _patientsFullname { get; }
        private ScheduleRepository _scheduleRepository;
        public IEnumerable<String> Patients => _patientsFullname;
        private PatientRepository _patientRepository;
        private Doctor _dr;
        private Patient _patient;
        private AppointmentViewModel _appointmentModel;
        

        public DrChangeAppointmentViewModel(AppointmentViewModel appointmentModel, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository, ObservableCollection<AppointmentViewModel> appointment, PatientRepository patientRepository, Doctor doctor,Patient patient,AppointmentViewModel appointmentSelected)
        {
            _dr = doctor;
            _appointmentModel = appointmentModel;
            _patient = patient;
            _appointmentModel = appointmentSelected;
            _scheduleRepository = scheduleRepository;
            _patientRepository = patientRepository;
            PatientRepository _controller = new PatientRepository();
            List<Patient> patients = _controller.Patients;

            _patientsFullname = new ObservableCollection<string>();
            foreach (Patient p in patients)
            {
                _patientsFullname.Add(p.FullName + "-" + p.Email);
            }

            _startDateChange = appointmentModel.Date;
            _changeHours = appointmentModel.Date.Hour;
            _changeMinutes = appointmentModel.Date.Minute;
            ChangeCommand = new DelegateCommand(o => DrChangeAppointment(appointment));
        }




        private DateTime _startDateChange;
        public DateTime StartDateChange
        {
            get
            {
                return _startDateChange;
            }
            set
            {
                _startDateChange = value;
                OnPropertyChanged(nameof(StartDateChange));
            }
        }

        private int _changeHours;
        public int ChangeHours
        {
            get
            {
                return _changeHours;
            }
            set
            {
                ChangeHours = value;
                OnPropertyChanged(nameof(ChangeHours));
            }
        }
        private int _changeMinutes;
        public int ChangeMinutes
        {
            get
            {
                return _changeMinutes;
            }
            set
            {
                _changeMinutes = value;
                OnPropertyChanged(nameof(ChangeMinutes));
            }
        }



        public ICommand ChangeCommand { get; }
        public ICommand CancelCommand { get; }

        public void DrChangeAppointment(ObservableCollection<AppointmentViewModel> Appointments)
        {
            try
            {
                int hours = ChangeHours;
                int minutes = ChangeMinutes;
                DateTime d = StartDateChange;

                DateTime start = new DateTime(d.Year, d.Month, d.Day, hours, minutes, 0);
                DateTime end = start.AddMinutes(15);
                TimeSlot time = new TimeSlot(start, end);

                string patientName = _patient.FirstName;
            

                MedicalRecord medicalRecord = new MedicalRecord(_patient);

                
                Appointment appointment = _scheduleRepository.ChangeAppointment(_appointmentModel.Id,time, _dr, medicalRecord);


                if (appointment != null)
                {
                    Appointments.Remove(_appointmentModel);
                    Appointments.Add(new AppointmentViewModel(appointment));
                }
                else
                {
                    MessageBox.Show("Invalid Appointment", "Error", MessageBoxButton.OK);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Invalid Appointment", "Error", MessageBoxButton.OK);
            }
        }
    }
}
