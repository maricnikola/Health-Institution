using System.Windows;
using ZdravoCorp.Core.Loader;
using ZdravoCorp.Core.Repositories.Equipment;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            //Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            //Load functions for repositories
            IDGenerator idg = new IDGenerator();
            JobScheduler scheduler = new JobScheduler();
            UserRepository userRepository = new UserRepository();
            DirectorRepository directorRepository = new DirectorRepository();
            PatientRepository patientRepository = new PatientRepository();
            NurseRepository nurseRepository = new NurseRepository();
            DoctorRepository doctorRepository = new DoctorRepository();
            EquipmentRepository equipmentRepository = new EquipmentRepository();
            RoomRepository roomRepository = new RoomRepository();
            InventoryRepository inventoryRepository = new InventoryRepository(roomRepository, equipmentRepository);
            MedicalRecordRepository medicalRecordRepository =  new MedicalRecordRepository();
            ScheduleRepository scheduleRepository = new ScheduleRepository();
            OrderRepository orderRepository = new OrderRepository();


            //___________________________
            var dialog = new LoginDialog(userRepository, patientRepository, doctorRepository, scheduleRepository, inventoryRepository, orderRepository, medicalRecordRepository);
            
            if (dialog.ShowDialog() == true)
            {
               
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                dialog.Close();
                if (Current.MainWindow != null) Current.MainWindow.Show();
            }
            else
            {
                MessageBox.Show("Invalid login.", "Error", MessageBoxButton.OK);
                Current.Shutdown(-1);
            }
        }
    }
}
