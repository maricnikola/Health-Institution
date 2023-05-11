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
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.TimeSlots;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels
{
    class DrChangeAppointmentViewModel : ViewModelBase
    {
        private ObservableCollection<string> _patientsFullname { get; }
        private ScheduleRepository _scheduleRepository;
        public IEnumerable<string> Patients => _patientsFullname;
        private PatientRepository _patientRepository;
        private Doctor _dr;
        private Patient _patient;
        private AppointmentViewModel _appointmentModel;
        private DateTime _date;
        public int[] PossibleMinutes { get; set; }
        public int[] PossibleHours { get; set; }


        public DrChangeAppointmentViewModel(AppointmentViewModel appointmentModel, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository, ObservableCollection<AppointmentViewModel> appointment, PatientRepository patientRepository, Doctor doctor, Patient patient, AppointmentViewModel appointmentSelected, DateTime date)
        {
            _dr = doctor;
            _appointmentModel = appointmentModel;
            _patient = patient;
            _appointmentModel = appointmentSelected;
            _scheduleRepository = scheduleRepository;
            _patientRepository = patientRepository;
            _date = date;
            PatientRepository _controller = new PatientRepository();
            List<Patient> patients = _controller.Patients;

            _patientsFullname = new ObservableCollection<string>();
            foreach (Patient p in patients)
            {
                _patientsFullname.Add(p.FullName + "-" + p.Email);
            }

            _startDateChange = appointmentModel.Date;
            _startTimeHours = _appointmentModel.Date.Hour;
            _startTimeMinutes = _appointmentModel.Date.Minute;
            PossibleMinutes = new[] { 00, 15, 30, 45 };
            PossibleHours = new[]
                { 00, 01, 02, 03, 04, 05, 06, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            ChangeCommand = new DelegateCommand(o => DrChangeAppointment(appointment));
            CancelCommand = new DelegateCommand(o => CloseWindow());
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

        private int _startTimeHours = 00;
        public int StartTimeHours
        {
            get
            {
                return _startTimeHours;
            }
            set
            {
                _startTimeHours = value;
                OnPropertyChanged(nameof(StartTimeHours));
            }
        }
        private int _startTimeMinutes = 00;
        public int StartTimeMinutes
        {
            get
            {
                return _startTimeMinutes;
            }
            set
            {
                _startTimeMinutes = value;
                OnPropertyChanged(nameof(StartTimeMinutes));
            }
        }



        public ICommand ChangeCommand { get; }
        public ICommand CancelCommand { get; }
        private void CloseWindow()
        {
            Window activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            activeWindow?.Close();
        }

        public void DrChangeAppointment(ObservableCollection<AppointmentViewModel> Appointments)
        {
            try
            {
                int hours = StartTimeHours;
                int minutes = StartTimeMinutes;
                DateTime d = StartDateChange;

                DateTime start = new DateTime(d.Year, d.Month, d.Day, hours, minutes, 0);
                DateTime end = start.AddMinutes(15);
                TimeSlot time = new TimeSlot(start, end);

                string patientName = _patient.FirstName;


                MedicalRecord medicalRecord = new MedicalRecord(_patient);


                Appointment appointment = _scheduleRepository.ChangeAppointment(_appointmentModel.Id, time, _dr, _patient.Email);


                if (appointment != null)
                {
                    CloseWindow();
                    if (_scheduleRepository.IsForShow(appointment, _date))
                    {
                        Appointments.Remove(_appointmentModel);
                        Appointments.Add(new AppointmentViewModel(appointment));

                    }
                    else
                    {
                        Appointments.Remove(_appointmentModel);
                    }
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
