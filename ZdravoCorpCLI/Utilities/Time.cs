using System.Globalization;

namespace ZdravoCorpCLI.Utilities;

public static class Time
{
    public static DateTime GetStartDateTime(string usage)
    {
        DateTime date;
        do
        {
            Console.Write($"Enter a {usage} start date (yyyy-MM-dd): ");
            var dateInput = Console.ReadLine();
            if (!DateTime.TryParse(dateInput, out date) || date < DateTime.Now.Date)
            {
                WriteError("Invalid date. Please enter a valid date in the future.");
                continue;
            }

            while (true)
            {
                Console.Write("Enter the start time (HH:mm): ");
                var startTimeInput = Console.ReadLine();
                if (!TimeSpan.TryParseExact(startTimeInput, "hh\\:mm", CultureInfo.CurrentCulture,
                        out TimeSpan startTime))
                {
                    WriteError("Invalid start time. Please enter a valid time in the format HH:mm.");
                    continue;
                }

                return new DateTime(date.Year, date.Month, date.Day, startTime.Hours, startTime.Minutes, 0);
            }
        } while (true);
    }

    public static DateTime GetEndDateTime(DateTime start, string usage)
    {
        DateTime date;
        do
        {
            Console.Write($"Enter a {usage} end date (yyyy-MM-dd): ");
            var dateInput = Console.ReadLine() + " 23:59:59";
            if (!DateTime.TryParse(dateInput, out date) || date <= DateTime.Now.Date || date < start)
            {
                WriteError("Invalid date. Please enter a valid date in the future.");
                continue;
            }

            while (true)
            {
                Console.Write("Enter the end time (HH:mm): ");
                var startTimeInput = Console.ReadLine();
                if (!TimeSpan.TryParseExact(startTimeInput, "hh\\:mm", CultureInfo.CurrentCulture,
                        out TimeSpan startTime))
                {
                    WriteError("Invalid end time. Please enter a valid time in the format HH:mm.");
                    continue;
                }

                var endDate = new DateTime(date.Year, date.Month, date.Day, startTime.Hours, startTime.Minutes, 0);
                if (endDate <= start)
                {
                    WriteError($"Invalid end time. Please enter a valid after {usage} start time");
                    continue;
                }

                return endDate;
            }
            
        } while (true);
    }

    public static void WriteError(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error);
        Console.ResetColor();
    }
}