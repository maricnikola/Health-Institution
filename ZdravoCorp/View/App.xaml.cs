using System.Windows;
using ZdravoCorp.Core.Repositories.Equipment;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Repositories.User;

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
            UserRepository userRepository = new UserRepository();
            DirectorRepository directorRepository = new DirectorRepository();
            PatientRepository patientRepository = new PatientRepository();
            NurseRepository nurseRepository = new NurseRepository();
            DoctorRepository doctorRepository = new DoctorRepository();
            EquipmentRepository equipmentRepository = new EquipmentRepository();
            RoomRepository roomRepository = new RoomRepository();
            InventoryRepository inventoryRepository = new InventoryRepository(roomRepository, equipmentRepository);




            //___________________________
            var dialog = new LoginDialog(userRepository,doctorRepository, inventoryRepository);
            
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
