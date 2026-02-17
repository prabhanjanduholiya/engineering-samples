using learn.dotnet.IComparable_Vs_IComparer.Comparable;
using System.Collections.Generic;

namespace learn.dotnet.IComparable_Vs_IComparer
{
    public static class StaticData
    {
        public static List<Employee> Employees = new List<Employee>
        {
            new Employee()
            {
                Id= 1,
                Age = 40,
                FirstName = "Brian",
                LastName = "Adam"
            },

            new Employee()
            {
                Id= 2,
                Age = 10,
                FirstName = "Billion",
                LastName = "Games"
            },

            new Employee()
            {
                Id= 3,
                Age = 80,
                FirstName = "Gomes",
                LastName = "Smith"
            },
            new Employee()
            {
                Id= 4,
                Age = 30,
                FirstName = "Hilery",
                LastName = "Cl"
            },
            new Employee()
            {
                Id= 5,
                Age = 40,
                FirstName = "Bill",
                LastName = "Pl"
            }
        };

        public static List<ComparableEmployee> ComparableEmployees = new List<ComparableEmployee>
        {
            new ComparableEmployee()
            {
                Id= 1,
                Age = 40,
                FirstName = "Brian",
                LastName = "Adam"
            },

            new ComparableEmployee()
            {
                Id= 2,
                Age = 10,
                FirstName = "Billion",
                LastName = "Games"
            },

            new ComparableEmployee()
            {
                Id= 3,
                Age = 80,
                FirstName = "Gomes",
                LastName = "Smith"
            },
            new ComparableEmployee()
            {
                Id= 4,
                Age = 30,
                FirstName = "Hilery",
                LastName = "Cl"
            },
            new ComparableEmployee()
            {
                Id= 5,
                Age = 40,
                FirstName = "Bill",
                LastName = "Pl"
            }
        };
    }
}
