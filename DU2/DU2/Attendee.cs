namespace DU2;

public class Attendee
{
    public string Name { get;  }

    public Attendee(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}