using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Learn.DotNet.Puzzles.Tests
{
    [TestClass]
    public class OpenCloseBracesPatternIdentificationTest
    {
        [TestMethod()]
        [DataRow("[][][[]]][[]")]
        [DataRow("][][][][")]
        public void IsInvalidStringTest(string input)
        {
            Assert.ThrowsException<Exception>(() => OpenCloseBracesPatternIdentification.IsValidString(input));
        }

        [TestMethod()]
        [DataRow("[][][][]")]
        [DataRow("[][[][]][]")]
        [DataRow("[][[[]][]][]")]
        public void IsValidStringTest(string input)
        {
            var result = OpenCloseBracesPatternIdentification.IsValidString(input);

            Assert.IsTrue(result);
        }
    }
}
