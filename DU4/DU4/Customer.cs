using System.Xml.Serialization;

namespace DU4;

public class Customer : EventArgs,IComparable
{
    public string? Name { get; set; }
    public int Age { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Age})";
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Customer customer)
            return 0;

        if (Name == customer.Name)
           return Age.CompareTo(customer.Age);

        if (Name == null)
        {
            return -1;
        }

        return Name.CompareTo(customer.Name);
    }

    public static Customer LoadCustomer(BinaryReader br)
    {
        return new Customer()
        {
            Name = br.ReadString(),
            Age = br.ReadInt32()
        };
    }

}