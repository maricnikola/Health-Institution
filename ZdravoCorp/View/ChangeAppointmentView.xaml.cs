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
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for ChangeAppointmentView.xaml
    /// </summary>
    public partial class ChangeAppointmentView : Window
    {
        public ChangeAppointmentView(AppointmentViewModel appointmentViewModel, DoctorRepository drRepository, ScheduleRepository scheduleRepository, ObservableCollection<AppointmentViewModel> Appointments, Patient patient)
        {
            ChangeAppointmentViewModel CAVM = new ChangeAppointmentViewModel(appointmentViewModel ,drRepository.GetAll(), scheduleRepository, Appointments, drRepository, patient);
            DataContext = CAVM;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
