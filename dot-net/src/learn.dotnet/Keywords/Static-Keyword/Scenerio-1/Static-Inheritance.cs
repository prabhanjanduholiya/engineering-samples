using System;
using System.Collections.Generic;

/// <summary>
/// Let's suppose,
/// *Base class has one static and one non-static CTOR.
/// * Child class also has one static and one non-static CTOR.
/// What will be the order of execution?
/// </summary>
namespace Learn.DotNet.Keywords.Static_Keyword.Scenerio_1
{
    public class Parent
    {
        public static IList<string> Messages = new List<string>();

        static Parent()
        {
            Messages.Add("Parent class static CTOR");
            Console.WriteLine("Parent class static CTOR");
        }

        public Parent()
        {
            Messages.Add("Parent class non-static CTOR");
            Console.WriteLine("Parent class non-static CTOR");
        }
    }

    public class Child : Parent
    {
        static Child()
        {
            Messages.Add("Child class static CTOR");
            Console.WriteLine("Child class static CTOR");
        }

        public Child()
        {
            Messages.Add("Child class non-static CTOR");
            Console.WriteLine("Child class non-static CTOR");
        }
    }
}
