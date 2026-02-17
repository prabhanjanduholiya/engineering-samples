using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Learn.DotNet.Puzzles.Tests
{
    [TestClass]
    public class PalindromProblemTest
    {
        [TestMethod()]
        public void IsPalindromStringTest()
        {
            var result = PalindromProblem.IsPalindromString("iTopiNonAvevanoNipoti");

            Assert.IsTrue(result);
        }

        [TestMethod()]
        [DataRow("duholiya", false)]
        public void IsNotPalindromStringTest(string input, bool expectedResult)
        {
            var result = PalindromProblem.IsPalindromString(input);

            Assert.AreEqual(expectedResult, result);
        }
    }
}




