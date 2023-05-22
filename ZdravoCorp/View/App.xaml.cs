using System.Windows;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;

namespace ZdravoCorp.View;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void ApplicationStart(object sender, StartupEventArgs e)
    {
        //Disable shutdown when the dialog closes
        Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        var idg = new IDGenerator();
        Injector.Configure();
        var scheduler = new JobScheduler();

        
        var dialog = new LoginDialog();

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