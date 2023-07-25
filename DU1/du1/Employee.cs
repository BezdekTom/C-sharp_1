

namespace du1
{
    public class Employee
    {

        #region Properties

        public string Name { get; }
        public int? Age { get;  }
        public PhoneNumber? PhoneNumber { get;  }
        public int Income { get; }
        public bool? IsActive { get;  }

        #endregion

        public Employee(string name, int? age, PhoneNumber? phoneNumber, int income, bool? isActive)
        {
            Name = name;
            Age = age;
            PhoneNumber = phoneNumber;
            Income = income;
            IsActive = isActive;
        }

        public override string ToString()
        {
            return $"{Name} | {PhoneNumber}";
        }

    }
}