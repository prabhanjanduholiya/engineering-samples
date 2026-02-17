using learn.dotnet.IComparable_Vs_IComparer.Comparer;
using System;

namespace learn.dotnet.IComparable_Vs_IComparer
{
    public class Demo
    {
        public static void Run()
        {
            Console.WriteLine("-----------------Sorting of complex type without IComparable/IComparer---------------------");

            try
            {
                var employees = EmployeeManager.SortWithoutComparisionInterface(StaticData.Employees);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message} ");
            }

            Console.WriteLine("-----------------Usage of IComparable---------------------");

            var sortedList = EmployeeManager.SortUsingIComparable(StaticData.ComparableEmployees);

            foreach (var item in sortedList)
            {
                Console.WriteLine($" {item.FirstName} {item.LastName} {item.Age} {item.Id}");
            }

            Console.WriteLine("-----------------Usage of IComparer for sort by age---------------------");

            var sortedListByAge = EmployeeManager.SortWithIComparerInterface(StaticData.Employees, new SortByAgeComparer());

            foreach (var item in sortedListByAge)
            {
                Console.WriteLine($" {item.FirstName} {item.LastName} {item.Age} {item.Id}");
            }

            Console.WriteLine("-----------------Usage of IComparer for sort by Last Name---------------------");

            var sortedListByLastName = EmployeeManager.SortWithIComparerInterface(StaticData.Employees, new SortByLastNameComparer());

            foreach (var item in sortedListByLastName)
            {
                Console.WriteLine($" {item.FirstName} {item.LastName} {item.Age} {item.Id}");
            }

            Console.WriteLine("--------------------Press any key to terminate-----------------------------------");

            Console.ReadLine();
        }
    }
}
