using System;

namespace learn.dotnet.IComparable_Vs_IComparer.Comparable
{
    public class ComparableEmployee : IComparable<ComparableEmployee>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public int CompareTo(ComparableEmployee obj)
        {
            return string.Compare(this.FirstName, obj.FirstName);
        }
    }
}
