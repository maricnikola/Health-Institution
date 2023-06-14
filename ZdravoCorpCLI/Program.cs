// See https://aka.ms/new-console-template for more information

using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Autofac;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Utilities.CronJobs;
using ZdravoCorp.Core.ViewModels;
using ZdravoCorpCLI;
using ZdravoCorpCLI.View;


public class Program
{
    private static IScheduleService? _scheduleService;
    private static IDoctorService? _doctorService;
    private static ObservableCollection<Doctor> doctors = new ObservableCollection<Doctor>();
    private static DateTime _date = DateTime.Now + TimeSpan.FromHours(1);
    private static Doctor? _chosenDoctor;
    private static string? _doctorName;
    private static int _startHours;
    private static int _startMinutes;
    private static int _endHours;
    private static int _endMinutes;
    private static string? _priority;

    static void Main()
    {
        var idg = new IDGenerator();
        Injector.Configure();
        var scheduler = new JobScheduler();

        Console.WriteLine("-----Bonus functionalities-----\n");
        var option = "";
        do
        {
            Console.WriteLine(" 1. Complex renovations\n 2. Recommend appointment\n 3. Exit");
            Console.Write("Enter option number: ");
            option = Console.ReadLine();
        } while (option != "1" && option != "2" && option != "3");

        switch (option)
        {
            case "1":
                Console.Clear();
                var complexRenovations = new ComplexRenovationsView();
                complexRenovations.Run();
                break;
            case "2":
                break;
            case "3":
                return;
        }
    }
}
