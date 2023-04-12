using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZdravoCorp.Core.Appointments.Entities;
using ZdravoCorp.Core.Appointments.Model;
using ZdravoCorp.Core.Loader;
using ZdravoCorp.Core.Schedule.Repository;
using ZdravoCorp.Core.User;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for AppointmentTableView.xaml
    /// </summary>
    public partial class AppointmentTableView : Window
    {

        public ScheduleRepository _controller;
        //public ObservableCollection<Appointment> Appointments { get; set; }
        public AppointmentModel model;
        public Appointment SelectedAppointment { get; set; }

        public AppointmentTableView(Doctor doctor)
        {
            _controller = new ScheduleRepository();
            LoadFunctions.LoadAppointments(_controller);
            //_controller.SaveAppointments();
            DataContext = this;
            InitializeComponent();
            model = new AppointmentModel("aaa");
            //Appointments = new ObservableCollection<Appointment>(_controller.GetDoctorAppointments(doctor));

        }

    }
}
