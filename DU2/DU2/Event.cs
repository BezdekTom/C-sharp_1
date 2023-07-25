using System.Globalization;

namespace DU2;

public abstract class Event
{
    #region Properties

    public DateTime Start { get; }
    public DateTime End { get;  }
    public string Name { get; }
    public bool IsUpcoming => (Start > DateTime.Now);

    #endregion

    #region Methods

    protected Event(string name, DateTime start, DateTime end)
    {
        Name = name;
        Start = start < end ? start : end;
        End = start < end ? end : start;
    }

    /// <summary>
    /// Returns time when calendar user will be reminded about event.
    /// </summary>
    /// <returns><see cref="DateTime"/> when user will be reminded, or null.</returns>
    public abstract DateTime? GetReminderTime();

    /// <summary>
    /// Return pair of dates from string. 
    /// </summary>
    /// <param name="text">String containing two dates separated by semicolon.</param>
    /// <returns>Two <see cref="DateTime"/> as tuple.</returns>
    /// <exception cref="InvalidDataException"></exception>
    protected static (DateTime start, DateTime end) TextToStartAndStop(string text)
    {
        var dates = text.Split(new[] { "\n", ";" }, StringSplitOptions.RemoveEmptyEntries);
        if (dates.Length != 2)
        {
            throw new InvalidDataException($"Invalid time format: {text}");
        }
        var cultureInfo = CultureInfo.GetCultureInfo("cs_CZ");
        return (DateTime.Parse(dates[0], cultureInfo), DateTime.Parse(dates[1], cultureInfo));
    }


    public override string ToString()
    {
        return $"{Name}: {Start} - {End}";
    }

    #endregion
}