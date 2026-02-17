using learn.dotnet.IComparable_Vs_IComparer;
using System;

namespace learn.dotnet.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----------------Sorting of complex type with IComparable/IComparer---------------------");

            Demo.Run();

            Console.ReadLine();
        }
    }
}
