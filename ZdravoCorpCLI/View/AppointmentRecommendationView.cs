using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.Scheduling.Models;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.GUI.Scheduling.ViewModels;
using ZdravoCorpCLI.Utilities;

namespace ZdravoCorpCLI.View;

public class AppointmentRecommendationView
{
    private IScheduleService? _scheduleService;
    private IDoctorService? _doctorService;
    private IPatientService? _patientService;
    private List<Doctor> _allDoctors;
    private List<string> _doctorList;
    private Doctor _chosenDoctor;
    private DateTime _lastDate;
    private DateTime _startTime;
    private DateTime _endTime;
    private string _priority;
    private string _patientEmail;

    public AppointmentRecommendationView()
    {
        _scheduleService = Injector.Container.Resolve<IScheduleService>();
        _doctorService = Injector.Container.Resolve<IDoctorService>();
        _patientService = Injector.Container.Resolve<IPatientService>();
        PopulateDoctorCollections();
    }

    private void PopulateDoctorCollections()
    {
        _allDoctors = _doctorService?.GetAll();
        _doctorList = new List<string>();
        foreach (var doctor in _allDoctors)
        {
            _doctorList.Add(doctor.FullName);
        }
    }

    public void Run()
    {
        Console.WriteLine("Get recommentded appointments");
        var option = "";
        do
        {
            Console.WriteLine(" 1. Recommend\n 2. Exit");
            Console.Write("Enter option number: ");
            option = Console.ReadLine();
        } while (option != "1" && option != "2");

        switch (option)
        {
            case "1":
                InputUsername();
                ChooseDoctor();
                ChooseLastDate();
                ChooseTimes();
                ChoosePriority();
                RecommendAppointemtns();
                break;
            case "2":
                Environment.Exit(0);
                break;
        }

    }

    private void InputUsername()
    {
        while (true)
        {
            Console.WriteLine("Enter your email: ");
            var email = Console.ReadLine();
            if (_patientService.GetByEmail(email) == null)
            {
                Time.WriteError("Invalid email! Try again");
                continue;
            }
            _patientEmail = email;
            break;
        }

    }
    private void ChooseDoctor()
    {
        Console.WriteLine("Available Doctors:");
        for (var i = 0; i < _doctorList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_doctorList[i]}");
        }
        int doctorIndex;
        do
        {
            Console.Write("Enter the number of the the desired doctor: ");
            var doctorInput = Console.ReadLine();
            if (!int.TryParse(doctorInput, out doctorIndex) || doctorIndex < 1 || doctorIndex > _allDoctors.Count)
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
        } while (doctorIndex < 1 || doctorIndex > _allDoctors.Count);
        _chosenDoctor = _allDoctors[doctorIndex - 1];
    }

    private void ChooseLastDate()
    {
        while (true)
        {
            Console.Write($"Enter a last date (yyyy-MM-dd): ");
            var dateInput = Console.ReadLine();
            if (DateTime.TryParse(dateInput, out _lastDate) && _lastDate >= DateTime.Now.Date) return;
            Time.WriteError("Invalid date. Please enter a valid date in the future.");
        }
    }

    private void ChooseTimes()
    {
        for (int i = 0; i < 2; i++)
        {
            while (true)
            {
                Console.Write(i == 0 ? "Enter the start time (HH:mm): " : "Enter the end time (HH:mm): ");
                var startTimeInput = Console.ReadLine();
                if (!TimeSpan.TryParseExact(startTimeInput, "hh\\:mm", CultureInfo.CurrentCulture,
                        out TimeSpan time))
                {
                    Time.WriteError("Invalid time. Please enter a valid time in the format HH:mm.");
                    continue;
                }

                if (i == 0)
                {
                    _startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.Hours,
                        time.Minutes, 0);
                    _startTime = TimeSlot.GiveFirstDevisibleBy15(_startTime);
                }
                else
                {
                    _endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.Hours,
                        time.Minutes, 0);
                    _endTime = TimeSlot.GiveFirstDevisibleBy15(_endTime);
                    if (_endTime <= _startTime)
                    {
                        Time.WriteError($"Invalid end time. Please enter a time that is afrer {_startTime}");
                        continue;
                    }
                }
                break;
            }
        }
    }
    private void ChoosePriority()
    {
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
                _priority = priorityIndex == 1 ? "Doctor" : "Time";
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter either 1 or 2.");
            }
        } while (priorityIndex != 1 && priorityIndex != 2);
    }

    private void RecommendAppointemtns()
    {
        try
        {
            var wantedTimeSlot = new TimeSlot(_startTime, _endTime);
            var lastDate = new DateTime(_lastDate.Year, _lastDate.Month, _lastDate.Day, 23, 59, 0);
            Console.WriteLine("# | " + "{0,-25} | {1, -25} | {2, -25}", "DATE", "DOCTOR", "PATIENT");
            Console.WriteLine(new string('-', 105));
            List<Appointment> fittingAppointments;
            if (_priority == "Doctor")
            {
                fittingAppointments =
                    _scheduleService?.FindAppointmentsByDoctorPriority(_chosenDoctor, wantedTimeSlot, lastDate,
                        _patientEmail);
                for (int i = 0; i < fittingAppointments!.Count; i++)
                {
                    Console.WriteLine($"{i + 1} | " + new AppointmentViewModel(fittingAppointments[i]) + "\n");
                }
            }
            else
            {
                fittingAppointments =
                    _scheduleService?.FindAppointmentsByTimePriority(_chosenDoctor, wantedTimeSlot, lastDate,
                        _patientEmail, _doctorService);
                for (int i = 0; i < fittingAppointments!.Count; i++)
                {
                    Console.WriteLine($"{i + 1} | " + new AppointmentViewModel(fittingAppointments[i]) + "\n");
                }
            }
            MakeAppointment(fittingAppointments);
            Run();
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }

    private void MakeAppointment(List<Appointment> appointments)
    {
        var appointmentIndex = GetAppointmentIndex(appointments);
        if (appointmentIndex == -1)
            return;
        if (!_scheduleService.IsDoctorAvailable(appointments[appointmentIndex].Time,
                appointments[appointmentIndex].Doctor.Email) ||
            !_scheduleService.IsPatientAvailable(appointments[appointmentIndex].Time,
                appointments[appointmentIndex].Doctor.Email)) return;

        var appointmentDto = new AppointmentDTO(appointments[appointmentIndex].Id,
            appointments[appointmentIndex].Time.Start, appointments[appointmentIndex].Doctor,
            appointments[appointmentIndex].PatientEmail, appointments[appointmentIndex].Anamnesis);
        _scheduleService.AddAppointment(appointmentDto);
        Console.WriteLine("\nAppontment created successfully!\n");
    }
    private static int GetAppointmentIndex(List<Appointment> appointments)
    {
        int appointmentIndex;
        do
        {
            Console.Write("Enter the number of appointment you want to create (or x for exit): ");
            var appointmentInput = Console.ReadLine();
            if (appointmentInput.ToLower() == "x")
            {
                return -1;
            }
            if (!int.TryParse(appointmentInput, out appointmentIndex) || appointmentIndex < 1 ||
                appointmentIndex > appointments.Count)
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
        } while (appointmentIndex < 1 || appointmentIndex > appointments.Count);

        return appointmentIndex - 1;
    }

}