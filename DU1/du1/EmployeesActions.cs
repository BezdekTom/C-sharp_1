namespace du1;

public class EmployeesActions
{
    /// <summary>
    /// Create array of Employees from text from the .csv file.
    /// </summary>
    /// <param name="csvFileText">Text with data.</param>
    /// <returns>Array of employees those personal information were stored in .</returns>
    /// <exception cref="InvalidDataException"></exception>
    public Employee[] GetEmployeesFromCsv(string csvFileText)
    {
        var lines = csvFileText
                .Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
        lines = lines.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        if (lines.Length <= 0)
        {
            throw new InvalidDataException($"Invalid structure of data");
        }

        Employee[] employees = new Employee[lines.Length - 1];
        for (int i = 0; i < employees.Length; i++)
        {
            var attributes = lines[i + 1].Split(';');

            int? age = null;
            if (!string.IsNullOrEmpty(attributes[1]) && int.TryParse(attributes[1], out var ageValue))
            {
                age = ageValue;
            }

            var phoneNumber = PhoneNumber.Parse(attributes[2]);

            if (!int.TryParse(attributes[3], out var income))
            {
                throw new InvalidDataException(
                    $"Invalid value of attribute {nameof(income)} on line {i} of data:" +
                    $"{nameof(income)} = {income}");
            }

            bool? isActive = null;
            if (!string.IsNullOrEmpty(attributes[4]))
            {
                switch (attributes[4])
                {
                    case "ano":
                        isActive = true;
                        break;
                    case "ne":
                        isActive = false;
                        break;
                    default:
                        throw new InvalidDataException(
                            $"Invalid value of attribute {nameof(isActive)} on line {i} of data:" +
                            $"{nameof(isActive)} = {isActive}");
                }
            }

            employees[i] = new Employee(attributes[0], age, phoneNumber, income, isActive);
        }

        return employees;

    }

    /// <summary>
    /// Compute average age of all employees who have their age set.
    /// As <see cref="numberOfIgnoredEmployees"/> it returns number of employees, who does not have their age set.
    /// </summary>
    /// <param name="employees">Employees over those mean of age is computed.</param>
    /// <param name="numberOfIgnoredEmployees">Count of ignored employees, who don't have their name set.</param>
    /// <returns>Mean of age of employees with set age.</returns>
    public double? MeanOfAge(Employee[] employees, out int numberOfIgnoredEmployees)
    {
        numberOfIgnoredEmployees = employees.Count(e => !e.Age.HasValue);
        if (numberOfIgnoredEmployees == employees.Length)
            return null;

        return (double)employees.Sum(e =>e.Age ?? 0) /
               (double)(employees.Length - numberOfIgnoredEmployees);
    }

    /// <summary>
    /// Print name and phone number of employees, who fulfilled condition given in assignment.
    /// </summary>
    /// <param name="employees">Employees over who the condition filter is applied.</param>
    public void PrintFilteredEmployees(Employee[] employees)
    {
        Func<Employee, bool> predicate = e => (e.IsActive.HasValue && e.IsActive.Value && e.Income > 30000
                                              && (!e.PhoneNumber.HasValue || e.PhoneNumber.Value.areaCode != "+421"));

        var filteredEmployees = employees.Where(predicate);

        if (!filteredEmployees.Any())
        {
            Console.Out.WriteLine("Žádný zaměstnanec neodpovídá filtru.");
            return;
        }

        foreach (var employee in filteredEmployees)
        {
            Console.Out.WriteLine(employee.ToString());
        }
    }
}