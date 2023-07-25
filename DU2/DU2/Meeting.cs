namespace DU2;

public class Meeting : Event, IAttendees
{
    #region Fields and Properties

    private static readonly TimeSpan reminderTime = new TimeSpan(0, 0, 30, 0);
    private static readonly TimeSpan ateendeeReminderTime = new TimeSpan(0, 0, 10, 0);

    private Attendee[]? attendees;
    public Attendee[] Attendees => (attendees ?? Array.Empty<Attendee>());

    #endregion


    public Meeting(string name, DateTime start, DateTime end, Attendee[]? attendees) : base(name, start, end)
    {
        this.attendees = attendees;
    }

    #region Methods

    public override DateTime? GetReminderTime() => (Start - reminderTime - Attendees.Length * ateendeeReminderTime);

    /// <summary>
    /// Parse Meeting from given string.
    /// </summary>
    /// <param name="meetingDetails">Text in form from assignment.</param>
    /// <returns><see cref="Meeting"/> parsed from given text.</returns>
    /// <exception cref="InvalidDataException"></exception>
    public static Meeting Parse(string meetingDetails)
    {
        string[] lines = meetingDetails.Split("\n");
        if (lines.Length < 3)
        {
            throw new InvalidDataException($"Invalid format of Meeting text: {meetingDetails}");
        }

        var (start, end) = Event.TextToStartAndStop(lines[1]);

        Attendee[]? attendees = null;
        if (!string.IsNullOrEmpty(lines[2]) && !string.IsNullOrWhiteSpace(lines[2]))
        {
            var names = lines[2].Split(new string[] { "\n", ";" }, StringSplitOptions.RemoveEmptyEntries);
            attendees = names.Select(s => new Attendee(s)).ToArray();
        }

        return new Meeting(lines[0], start, end, attendees);
    }

    #endregion
}