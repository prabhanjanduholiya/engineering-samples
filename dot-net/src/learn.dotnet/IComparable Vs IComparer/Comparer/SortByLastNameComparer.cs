using System.Collections.Generic;

namespace learn.dotnet.IComparable_Vs_IComparer.Comparer
{
    public class SortByLastNameComparer : IComparer<Employee>
    {
        public int Compare(Employee x, Employee y)
        {
            return string.Compare(x.LastName, y.LastName);
        }
    }
}
