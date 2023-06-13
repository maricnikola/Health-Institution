using System.Globalization;
using Autofac;
using ZdravoCorp.Core.Models.Renovation;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Services.RenovationServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;
using ZdravoCorp.Core.ViewModels.DirectorViewModel;
using ZdravoCorpCLI.Utilities;

namespace ZdravoCorpCLI;

public class ComplexRenovations
{
    private IRoomService _roomService;
    private IRenovationService _renovationService;
    private IScheduleService _scheduleService;


    public ComplexRenovations()
    {
        _roomService = Injector.Container.Resolve<IRoomService>();
        _renovationService = Injector.Container.Resolve<IRenovationService>();
        _scheduleService = Injector.Container.Resolve<IScheduleService>();
    }


    public void Run()
    {
        Console.WriteLine("-----Complex Renovations-----\n");
        var option = "";
        do
        {
            Console.WriteLine(" 1. View Renovations\n 2. New Renovation\n 3. Exit");
            Console.Write("Enter option number: ");
            option = Console.ReadLine();
        } while (option != "1" && option != "2" && option != "3");

        switch (option)
        {
            case "1":
                ViewRenovations();
                break;
            case "2":
                NewRenovation();
                break;
            case "3":
                return;
        }

    }
    public void ViewRenovations()
    {
       Console.WriteLine("{0, -5} | {1, -25} | {2, -25} | {3, -25} | {4, -25} | {5, -15}", "Room", "Start", "Until", "Split", "Join", "Status");
       foreach (var renovation in _renovationService.GetAll())
       {
           Console.WriteLine(new RenovationViewModel(renovation));
       }
       Console.WriteLine("Press enter to go back!");
       while (Console.ReadKey().Key != ConsoleKey.Enter){}
       Run();
    }
    public void NewRenovation()
    {
        while (true)
        {
            var room = GetRoomForRenovation(_roomService.GetAll());
            if (room == null)
                return;
            var startDate = Time.GetStartDateTime("renovation");
            var endDate = Time.GetEndDateTime(startDate, "renovation");
            var timeslot = new TimeSlot(startDate, endDate);
            if (!ValidateRoomInTimeSlot(room, timeslot))
            {
                WriteError("Room has scheduled appointments or renovations in selected timeslot!");
                continue;
            }

            var renovationType = GetRenovationType();
            Room? split = null;
            Room? join = null;
            switch (renovationType)
            {
                case 1:
                    break;
                case 2:
                    var splitRoomType = GetSplitRoomType();
                    split = new Room(IDGenerator.GetId(), splitRoomType, false);
                    break;
                case 3:
                    while (true)
                    {
                        join = GetRoomForRenovation(_roomService.GetAllExcept(room.Id));
                        if (!ValidateRoomInTimeSlot(room, timeslot))
                        {
                            WriteError("Room has scheduled appointments or renovations in selected timeslot!");
                            continue;
                        }
                        break;
                    }
                    break;
            }

            BeginRenovation(new RenovationDTO(IDGenerator.GetId(), room, timeslot,
                Renovation.RenovationStatus.Pending, split, join));
            Console.WriteLine("Renovation scheduled successfully!");
            Console.WriteLine("Press enter to go back!");
            while (Console.ReadKey().Key != ConsoleKey.Enter){}
            Run();
        }
    }

    private void BeginRenovation(RenovationDTO renovationDto)
    {
        _renovationService.AddRenovation(renovationDto);
        JobScheduler.RenovationTaskScheduler(renovationDto);
    }

    private bool ValidateRoomInTimeSlot(Room room, TimeSlot timeSlot)
    {
        if (_scheduleService.CheckRoomAvailability(room.Id, timeSlot) &&
            !_renovationService.IsRenovationScheduled(room.Id, timeSlot))
        {
            return true;
        }

        return false;
    }

    private int GetRenovationType()
    {
        var option = "";
        do
        {
            Console.WriteLine(" 1. Basic\n 2. Split\n 3. Join");
            Console.Write("Enter option number: ");
            option = Console.ReadLine();
        } while (option != "1" && option != "2" && option != "3");

        return int.Parse(option);
    }

    private RoomType GetSplitRoomType()
    {
        var types = new Dictionary<string, RoomType>()
        {
            { "1", RoomType.ExaminationRoom },
            { "2", RoomType.OperationRoom },
            { "3", RoomType.PatientRoom },
            { "4", RoomType.WaitingRoom }
        };
        var option = "";
        do
        {
            Console.WriteLine(" 1. Examination\n 2. Operation\n 3. Patient\n 4. Waiting room");
            Console.Write("Enter option number: ");
            option = Console.ReadLine();
        } while (option != "1" && option != "2" && option != "3" && option != "4");

        return types[option];
    }

    private Room? GetRoomForRenovation(IEnumerable<Room> rooms)
    {
        var option = "";
        while (true)
        {
            Console.Clear();
            Console.WriteLine("{0,-5} | {1, -15} | {2, 10}", "ID", "TYPE", "RENOVATION");
            foreach (var room in rooms)
            {
                Console.WriteLine(new RoomViewModel(room));
            }

            Console.Write("Enter room ID: ");
            option = Console.ReadLine();
            if (option?.ToLower() == "x")
                return null;
            if (!rooms.Select(o => o.Id.ToString()).Contains(option))
                continue;

            if (_roomService.GetById(int.Parse(option)).IsUnderRenovation)
            {
                WriteError("Selected room is already under renovation!");
                continue;
            }

            return _roomService.GetById(int.Parse(option));
        }
    }


    private void WriteError(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error);
        Console.ResetColor();
    }
}