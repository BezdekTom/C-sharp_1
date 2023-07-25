namespace DU2;

public class Holiday : Event, IAttendees
{
    #region Fileds and Properties

    private Attendee[]? attendees;
    public Attendee[] Attendees => (attendees ?? Array.Empty<Attendee>());

    private string? location;
    public string Location
    {
        set => location = value;
        get => location ?? "Neznámé místo";
    }

    #endregion


    public Holiday(string name, DateTime start, DateTime end, Attendee[]? attendees = null) : base(name, start, end)
    {
        this.attendees = attendees;
    }

    #region Methods

    public override DateTime? GetReminderTime() => null;

    public override string ToString()
    {
        return $"{base.ToString()} ({Location})";
    }

    /// <summary>
    /// Parse Holiday from given string.
    /// </summary>
    /// <param name="holidayDetails">Text in form from assignment.</param>
    /// <returns><see cref="Holiday"/> parsed from given text.</returns>
    /// <exception cref="InvalidDataException"></exception>
    public static Holiday Parse(string holidayDetails)
    {
        string[] lines = holidayDetails.Split("\n");
        if (lines.Length < 4)
        {
            throw new InvalidDataException($"Invalid format of Holiday text: {holidayDetails}");
        }

        var (start, end) = Event.TextToStartAndStop(lines[1]);

        Attendee[]? attendees = null;
        if (!string.IsNullOrEmpty(lines[3]) && !string.IsNullOrWhiteSpace(lines[3]))
        {
            var names = lines[3].Split(new string[] { "\n", ";" }, StringSplitOptions.RemoveEmptyEntries);
            attendees = names.Select(s => new Attendee(s)).ToArray();
        }

        var holiday = new Holiday(lines[0], start, end, attendees);

        if (!string.IsNullOrEmpty(lines[2]) && !string.IsNullOrWhiteSpace(lines[2]))
        {
            holiday.Location = lines[2];
        }

        return holiday;
    }

    #endregion

}