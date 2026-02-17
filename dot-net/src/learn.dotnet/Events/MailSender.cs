using System;

namespace Learn.DotNet.Events
{
    public class MailSender
    {
        public static void DataParsingCompletedEventHandler(object source, EventArgs eventArgs)
        {
            Console.WriteLine("Data parsing completed event ...");
            Console.WriteLine("Mail sent...");
        }
    }
}
