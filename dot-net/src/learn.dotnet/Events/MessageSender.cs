using System;

namespace Learn.DotNet.Events
{
    public class MessageSender
    {
        public static void DataParsingCompletedEventHandler(object source, EventArgs eventArgs)
        {
            Console.WriteLine("Data parsing completed event ...");
            Console.WriteLine("Message sent...");
        }
    }
}
