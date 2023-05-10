using System.Collections.ObjectModel;
using System.Windows;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels;

namespace ZdravoCorp.View.PatientV
{
    /// <summary>
    /// Interaction logic for ChangeAppointmentView.xaml
    /// </summary>
    public partial class ChangeAppointmentView : Window
    {
        public ChangeAppointmentView()
        {
            //ChangeAppointmentViewModel CAVM = new ChangeAppointmentViewModel(appointmentViewModel ,drRepository.GetAll(), scheduleRepository, Appointments, drRepository, patient);
            //DataContext = CAVM;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
