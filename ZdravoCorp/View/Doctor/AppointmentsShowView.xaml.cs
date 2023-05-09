using System;
using System.Collections.Generic;
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
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels;

namespace ZdravoCorp.View.DoctorView
{
    /// <summary>
    /// Interaction logic for AppointmentsShowView.xaml
    /// </summary>
    public partial class AppointmentsShowView : Window
    {
        private ScheduleRepository _controller;
        
        
        public AppointmentsShowView()
        {
            //LoadFunctions.LoadAppointments(_controller);
            //List<Appointment> appointments = _controller.GetDoctorAppointments(doctor);
            //var make = new AppointmentShowViewModel(appointments);
            //DataContext = make;

            InitializeComponent();
        }
    }
}
