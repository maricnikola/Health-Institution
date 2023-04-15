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
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for AppointmentTableView.xaml
    /// </summary>
    public partial class AppointmentTableView : Window
    {
        private ScheduleRepository _controller;
        private DoctorRepository _doctorRepository;

        public AppointmentTableView(Patient patient,DoctorRepository dr)
        {
            _doctorRepository = dr;
            _controller = new ScheduleRepository();
            LoadFunctions.LoadAppointments(_controller);
            List<Appointment> appointments = _controller.GetPatientAppointments(patient);
            AppointmentTableViewModel VM = new AppointmentTableViewModel(appointments, _controller);

            DataContext = VM;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new MakeAppointmentView(_doctorRepository);
            window.Show();
        }
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
