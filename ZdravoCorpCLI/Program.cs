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
using ZdravoCorpCLI.View;

public class Program
{
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
        } while (option != "1" && option != "2" && option != "3" && option!= "4");

        switch (option)
        {
            case "1":
                Console.Clear();
                var complexRenovations = new ComplexRenovationsView();
                complexRenovations.Run();
                break;
            case "2":
                Console.Clear();
                AppointmentRecommendationView ar = new AppointmentRecommendationView();
                ar.Run();
                break;
            case "3":
                return;
            case "4":
                Console.Clear();
                var CRUDOptions = new CRUDAppointmentsOperationsView();
                CRUDOptions.Run();
                break;
        }
    }
}
    

