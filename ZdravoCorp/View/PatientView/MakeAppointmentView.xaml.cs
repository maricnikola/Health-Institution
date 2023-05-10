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
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for MakeAppointmentView.xaml
    /// </summary>
    public partial class MakeAppointmentView : Window
    {
        //private DoctorRepository _doctorRepository;
        public MakeAppointmentView()
        {
            //MakeAppointmentViewModel MAV = new MakeAppointmentViewModel(drRepository.GetAll(), scheduleRepository, Appointments, drRepository, patient);
            //DataContext = MAV;
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
