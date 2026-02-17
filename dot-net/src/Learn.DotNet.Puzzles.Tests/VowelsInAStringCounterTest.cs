using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn.DotNet.Puzzles.Tests
{
    [TestClass]
    public class VowelsInAStringCounterTest
    {
        [TestMethod()]
        [DataRow("Prabhanjan", 3)]
        public void VerifyVowelsCountInAString(string input, int expectedResult)
        {
            var actualResult = VowelsInAStringCounter.GetVowelCount(input);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
