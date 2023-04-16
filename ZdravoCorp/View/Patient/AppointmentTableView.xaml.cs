using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Core.Loader;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.Core.ViewModels;
using ZdravoCorp.Core.ViewModels.Patient;

namespace ZdravoCorp.View.Patient
{
    /// <summary>
    /// Interaction logic for AppointmentTableView.xaml
    /// </summary>
    public partial class AppointmentTableView : Window
    {
        private Core.Models.User.Patient _patient;
        private ScheduleRepository _controller;
        private DoctorRepository _doctorRepository;

        public AppointmentTableView(Core.Models.User.Patient patient,DoctorRepository dr, ScheduleRepository sr)
        {
            _patient = patient;
            _doctorRepository = dr;
            _controller = sr;
            //_controller = new ScheduleRepository();
            //LoadFunctions.LoadAppointments(_controller);
            List<Appointment> appointments = _controller.GetPatientAppointments(_patient);
            AppointmentTableViewModel VM = new AppointmentTableViewModel(appointments, _controller, _doctorRepository, _patient);

            DataContext = VM;
            InitializeComponent();
        }


        //private void Update()
        //{
        //    List<Appointment> appointments = _controller.GetPatientAppointments(_patient);
        //    AppointmentTableViewModel VM = new AppointmentTableViewModel(appointments, _controller);

        //    DataContext = VM;
        //}


        //public AppointmentTableView(Doctor doctor)
        //{
        //    _controller = new ScheduleRepository();
        //    LoadFunctions.LoadAppointments(_controller);
        //    List<Appointment> appointments = _controller.GetDoctorAppointments(doctor);
        //    AppointmentTableViewModel VM = new AppointmentTableViewModel(appointments);

        //    DataContext = VM;
        //    InitializeComponent();

        //}

    }
}
