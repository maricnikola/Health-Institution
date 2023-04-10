using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.Loader;
using ZdravoCorp.Core.User.Repository;
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
            //LoadFunctions.LoadUsers(userRepository);
            userRepository.LoadFromFile();
            //userRepository.SaveToFile();





            //___________________________
            var dialog = new LoginDialog(userRepository);

            if (dialog.ShowDialog() == true)
            {
                var mainWindow = new MainWindow();
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Invalid login.", "Error", MessageBoxButton.OK);
                Current.Shutdown(-1);
            }
        }
    }
}
