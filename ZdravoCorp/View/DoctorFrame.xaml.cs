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
using ZdravoCorp.Core.User;
using ZdravoCorp.Core.User.Repository;


namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for DoctorFrame.xaml
    /// </summary>
    public partial class DoctorFrame : Window
    {
        public Doctor _doctor;
        public DoctorRepository _controller;

        public DoctorFrame(User user)
        {

            _doctor = _controller.GetDoctorByEmail(user.Email);
            InitializeComponent();
        }


        private void Button_Click_Show(object sender, RoutedEventArgs e)
        {
            AppointmentTableView appointmentTable = new AppointmentTableView(_doctor);
            appointmentTable.Show();
        }
    }
}
