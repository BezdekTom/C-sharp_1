using System.Globalization;
using System.Text.RegularExpressions;

namespace DU2;

public class Calendar
{
    #region Fields

    private const string MeetingHeader = "MEETING:\n";
    private const string AppointmentHeader = "APPOINTMENT:\n";
    private const string HolidayHeader = "HOLIDAY:\n";

    private static readonly string verticalSeparator = "----------";

    private readonly List<Event> events = new();

    #endregion

    #region Methods

    public void Add(Event newEvent)
    {
        events.Add(newEvent);
    }

    public Event[] GetAllUpcoming()
    {
        return events.Where(e => e.IsUpcoming).ToArray();
    }

    /// <summary>
    /// Add all the events from given text.
    /// </summary>
    /// <param name="txtFile">String containing all the events, in format given by assignment.</param>
    /// <exception cref="InvalidDataException"></exception>
    public void AddEventsFromTxt(string txtFile)
    {
        string eventHeadersPattern = $"({MeetingHeader}|{AppointmentHeader}|{HolidayHeader})";
        var parts = Regex.Split(txtFile, eventHeadersPattern)
            .Where(s => (!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s))).ToArray();
        if (parts.Length % 2 != 0)
        {
            throw new InvalidDataException("Difference in number of event headers and event bodies.");
        }

        for (int i = 0; i < parts.Length; i += 2)
        {
            switch (parts[i])
            {
                case MeetingHeader:
                    Add(Meeting.Parse(parts[i + 1]));
                    break;
                case AppointmentHeader:
                    Add(Appointment.Parse(parts[i + 1]));
                    break;
                case HolidayHeader:
                    Add(Holiday.Parse(parts[i + 1]));
                    break;
                default:
                    throw new InvalidDataException("Invalid form of events input.");
            }
        }

    }

    /// <summary>
    /// Print all today events, today is set to 15.10. 2022.
    /// </summary>
    public void PrintTodayEvents()
    {
        var todayEvents = this[2022, 10, 15];
        Console.WriteLine("DNEŠNÍ UDÁLOSTI:");
        Console.WriteLine(verticalSeparator);
        foreach (var todayEvent in todayEvents)
        {
            Console.Out.WriteLine(todayEvent);
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Print all people user will meet today, today is set to 15.10. 2022.
    /// </summary>
    public void PrintPeopleIMeetToday()
    {
        var todayEvents = this[2022, 10, 15];
        Console.WriteLine("LIDÉ SE KTERÝMI SE DNES UVIDÍM:");
        Console.WriteLine(verticalSeparator);
        foreach (var todayEvent in todayEvents)
        {
            if (todayEvent is IAttendees)
            {
                var atendees = ((IAttendees)todayEvent).Attendees;
                foreach (var attendee in atendees)
                {
                    Console.Out.WriteLine($"{attendee} - {todayEvent.Start}");
                }
            }

        }
        Console.WriteLine();
    }

    /// <summary>
    /// Print all today reminders, today is set to 15.10. 2022.
    /// </summary>
    public void PrintTodayReminders()
    {
        var todayEvents = this[2022, 10, 15];
        Console.WriteLine("ČASY PŘIPOMENUTÍ DNEŠNÍCH UDÁLOSTÍ:");
        Console.WriteLine(verticalSeparator);
        foreach (var todayEvent in todayEvents)
        {
            Console.Out.WriteLine($"{todayEvent} => {todayEvent.GetReminderTime()}");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Print all events that did not start yet.
    /// </summary>
    public void PrintUpcomingEvents()
    {
        Console.WriteLine("BUDOUCÍ UDÁLOSTI:");
        Console.WriteLine(verticalSeparator);
        var upcomingEvents = this.GetAllUpcoming();
        foreach (var upcomingEvent in upcomingEvents)
        {
            Console.Out.WriteLine($"{upcomingEvent}");
        }
        Console.WriteLine();
    }

    #endregion


    /// <summary>
    /// Return all events that, interfere with given day
    /// </summary>
    /// <returns>Array of events interfere with given day.</returns>
    public Event[] this[int year, int month, int day]
    {
        get
        {
            var dateStart = new DateTime(year, month, day);
            var dateEnd = new DateTime(year, month, day).AddDays(1);

            return events.Where(e => (e.Start < dateEnd && e.End >= dateStart)).ToArray();

        }
    }
}