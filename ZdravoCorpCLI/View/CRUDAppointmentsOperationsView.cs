using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorpCLI.Utilities;

namespace ZdravoCorpCLI.View;

public class CRUDAppointmentsOperationsView
{
    private IScheduleService _scheduleService;
    private IDoctorService _doctorService;
    private IPatientService _patientService;
    private string _doctorMail;
    public CRUDAppointmentsOperationsView()
    {
        _scheduleService = Injector.Container.Resolve<IScheduleService>();
        _doctorService = Injector.Container.Resolve<IDoctorService>();
        _patientService = Injector.Container.Resolve<IPatientService>();
    }
    public void Run()
    {
        Console.WriteLine("CRUD operations for appointments and operations");
        InputUsername();
        ChooseOption();
    }
    private void InputUsername()
    {
        while (true)
        {
            Console.WriteLine("Enter your email: ");
            var email = Console.ReadLine();
            if (_doctorService.GetByEmail(email) == null)
            {
                Time.WriteError("Invalid email! Try again");
                continue;
            }
            _doctorMail = email;
            Console.WriteLine("You are logged in as " + _doctorMail);
            break;
        }

    }
    private void ChooseOption()
    {
        while (true)
        {
            Console.WriteLine("1. CRUD Operations\n2. CRUD Appointments\n>>");
            var input = Console.ReadLine();
            int command;
            if(!int.TryParse(input,out command) || command > 2 || command < 0)
            {
                Console.WriteLine("Invalid input. Please try again.");
                continue;
            }
            return;
        }
       
    }
    private void CRUDOperationsOptions()
    {
        while (true)
        {
            Console.WriteLine("1. Create operation\n2. Show operations\n3. Update operation\n4. Delete operation");
            var input = Console.ReadLine();
            int command;
            if (!int.TryParse(input, out command) || command > 2 || command < 0)
            {
                Console.WriteLine("Invalid input. Please try again.");
                continue;
            }
            return;
        }
    }
}
