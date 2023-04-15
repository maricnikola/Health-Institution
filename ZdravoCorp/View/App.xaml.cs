using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.Loader;
using ZdravoCorp.Core.Repositories.Equipment;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels;
using ZdravoCorp.View;

namespace ZdravoCorp
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
            ScheduleRepository scheduleRepository = new ScheduleRepository();
            LoadFunctions.LoadAppointments(scheduleRepository);



            //___________________________
            var dialog = new LoginDialog(userRepository,patientRepository,doctorRepository,scheduleRepository);
            
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
