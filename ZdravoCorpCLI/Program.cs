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
        var complexRenovations = new ComplexRenovations();
        complexRenovations.Run();
        /*_scheduleService = Injector.Container.Resolve<IScheduleService>();
        _doctorService = Injector.Container.Resolve<IDoctorService>();


        // Initialize Doctors collection
        
        var doctorsList = new List<string>();
        doctors = new ObservableCollection<Doctor>(_doctorService.GetAll()!);
        doctorsList.AddRange(doctors.Select(doctor => doctor.FullName));


        Console.WriteLine("Advanced Make Appointment");

        // Get doctor name
        Console.WriteLine("Available Doctors:");
        for (int i = 0; i < doctorsList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {doctorsList[i]}");
        }
        int doctorIndex;
        do
        {
            Console.Write("Enter the number corresponding to the desired doctor: ");
            var doctorInput = Console.ReadLine();
            if (!int.TryParse(doctorInput, out doctorIndex) || doctorIndex < 1 || doctorIndex > doctors.Count)
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
        } while (doctorIndex < 1 || doctorIndex > doctors.Count);
        _chosenDoctor = doctors[doctorIndex - 1];

        // Get date
        var validDate = false;
        do
        {
            Console.Write("Enter a date (yyyy-MM-dd): ");
            var dateInput = Console.ReadLine();
            if (DateTime.TryParse(dateInput, out _date) && _date >= DateTime.Now.Date)
            {
                validDate = true;
            }
            else
            {
                Console.WriteLine("Invalid date. Please enter a valid date in the future.");
            }
        } while (!validDate);

        // Get start time
        var validStartTime = false;
        do
        {
            Console.Write("Enter the start time (HH:mm): ");
            var startTimeInput = Console.ReadLine();
            if (TimeSpan.TryParse(startTimeInput, out TimeSpan startTime))
            {
                _startHours = startTime.Hours;
                _startMinutes = startTime.Minutes;
                validStartTime = true;
            }
            else
            {
                Console.WriteLine("Invalid start time. Please enter a valid time in the format HH:mm.");
            }
        } while (!validStartTime);

        // Get end time
        var validEndTime = false;
        do
        {
            Console.Write("Enter the end time (HH:mm): ");
            var endTimeInput = Console.ReadLine();
            if (TimeSpan.TryParse(endTimeInput, out TimeSpan endTime))
            {
                _endHours = endTime.Hours;
                _endMinutes = endTime.Minutes;
                if (_endHours > _startHours || (_endHours == _startHours && _endMinutes > _startMinutes))
                {
                    validEndTime = true;
                }
                else
                {
                    Console.WriteLine("Invalid end time. Please enter a time later than the start time.");
                }
            }
            else
            {
                Console.WriteLine("Invalid end time. Please enter a valid time in the format HH:mm.");
            }
        } while (!validEndTime);

        // Get priority
        int priorityIndex;
        do
        {
            Console.WriteLine("Select the priority:");
            Console.WriteLine("1. Doctor");
            Console.WriteLine("2. Time");
            Console.Write("Enter the number corresponding to the desired priority: ");
            var priorityInput = Console.ReadLine();
            if (int.TryParse(priorityInput, out priorityIndex) && (priorityIndex == 1 || priorityIndex == 2))
            {
                _priority = (priorityIndex == 1) ? "Doctor" : "Time";
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter either 1 or 2.");
            }
        } while (priorityIndex != 1 && priorityIndex != 2);

        // Invoke RecommendAppointments method
        RecommendAppointments();

    }

    private static void RecommendAppointments()
    {
        try
        {
            var startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, _startHours,
                _startMinutes, 0);
            var endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, _endHours, _endMinutes,
                0);
            var wantedTimeSlot = new TimeSlot(startTime, endTime);
            var lastDate = new DateTime(_date.Year, _date.Month, _date.Day, 23, 59, 0);

            if (_priority == "Doctor")
            {
                var fittingAppointments =
                    _scheduleService?.FindAppointmentsByDoctorPriority(_chosenDoctor, wantedTimeSlot, lastDate,
                        "sreten.pejovic@gmail.com");
                foreach (var appointmentForPrint in fittingAppointments.Select(singleAppointment => new AppointmentViewModel(singleAppointment)))
                {
                    Console.WriteLine(appointmentForPrint.Date + " - " + appointmentForPrint.DoctorEmail + " - "+ appointmentForPrint.PatientMail + "\n");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    */
    }
}
