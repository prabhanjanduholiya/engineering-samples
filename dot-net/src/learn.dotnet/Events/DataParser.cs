using System;
using System.Threading;

namespace Learn.DotNet.Events
{
    public class DataParser
    {
        // Here we used delegate that is provided by dotnet framework, we can create our own
        public event EventHandler<EventArgs> DataParsingCompleted;

        public void Parse(Data data)
        {
            Console.WriteLine("Parsing Data...");

            Thread.Sleep(3000);

            DataParsingComplted();
        }

        private void DataParsingComplted()
        {
            // Notify event subscribers
            if (DataParsingCompleted != null)
            {
                Console.WriteLine("Notifying subscribers....");
                DataParsingCompleted(this, EventArgs.Empty);
            }
        }
    }
}
