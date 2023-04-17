using System.Collections.ObjectModel;
using System.Windows;
using ZdravoCorp.Core.Models.User;
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
