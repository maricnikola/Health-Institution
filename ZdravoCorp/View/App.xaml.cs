using System.Windows;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Equipment;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.Transfers;
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
            RepositoryManager repositoryManager = new RepositoryManager();
            JobScheduler scheduler = new JobScheduler(repositoryManager.InventoryRepository, repositoryManager.TransferRepository, repositoryManager.OrderRepository);



            var dialog = new LoginDialog(repositoryManager);
            
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
