using System;

namespace DU2;

public class Appointment : Event
{
    private static readonly TimeSpan reminderTime = new TimeSpan(0, 0, 30, 0);

    public Appointment(string name, DateTime start, DateTime end) : base(name, start, end)
    { }

    public override DateTime? GetReminderTime() => (Start - reminderTime);

    /// <summary>
    /// Parse Appointment from given string.
    /// </summary>
    /// <param name="appointmentDetails">Text in form from assignment.</param>
    /// <returns><see cref="Appointment"/> parsed from given text.</returns>
    /// <exception cref="InvalidDataException"></exception>
    public static Appointment Parse(string appointmentDetails)
    {
        string[] lines = appointmentDetails.Split("\n");
        if (lines.Length < 2)
        {
            throw new InvalidDataException($"Invalid format of Appointment text: {appointmentDetails}");
        }
        var (start, end) = Event.TextToStartAndStop(lines[1]);
        return new Appointment(lines[0], start, end);
    }
}