using Learn.DotNet.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Learn.DotNet.Tests.Events
{
    [TestClass]
    public class DataParserTest
    {
        [TestMethod]
        public void Run()
        {
            Data data = new Data() { Text = "Raw Text" };

            DataParser dataParser = new DataParser();

            dataParser.DataParsingCompleted += MessageSender.DataParsingCompletedEventHandler;
            dataParser.DataParsingCompleted += MailSender.DataParsingCompletedEventHandler;

            dataParser.Parse(data);
        }
    }
}
