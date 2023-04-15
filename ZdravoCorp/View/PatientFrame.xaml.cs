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
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for PatientFrame.xaml
    /// </summary>
    public partial class PatientFrame : Window
    {
        private Patient _patient;
        private PatientRepository _controller;
        private DoctorRepository _doctorRepository;
        private ScheduleRepository _scheduleRepository;
        public PatientFrame(User user, PatientRepository patientRepository, DoctorRepository dr, ScheduleRepository sr)
        {
            _scheduleRepository = sr;
            _doctorRepository = dr;
            _controller = patientRepository;
            _patient = _controller.GetPatientByEmail(user.Email);
            InitializeComponent();
        }

        private void Button_Click_Show(object sender, RoutedEventArgs e)
        {
            AppointmentTableView appointmentTable = new AppointmentTableView(_patient,_doctorRepository, _scheduleRepository);
            appointmentTable.Show();
        }
    }
}
