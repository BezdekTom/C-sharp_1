namespace DU4;

public class AgeComparer : IComparer<Customer>
{
    public int Compare(Customer? x, Customer? y)
    {
        if (x is null && y is null)
            return 0;

        if (x is null)
            return -1;
        if (y is null)
            return 1;

        return x.Age.CompareTo(y.Age);

    }
}