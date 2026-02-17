using Learn.DotNet.Keywords.Static_Keyword.Scenerio_1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Learn.DotNet.Tests.Static_Keyword.Scenerio_1
{
    [TestClass]
    public class Static_InheritanceTest
    {
        [TestMethod]
        public void VerifyOrderOfConstructorExecutionUsingStaticWithinInheritance()
        {
            Child child = new Child();
            var messages = Parent.Messages;
            
            Assert.AreEqual("Parent class static CTOR", messages.ElementAt(0), "Execution order is other than expected");
            Assert.AreEqual("Child class static CTOR", messages.ElementAt(1), "Execution order is other than expected");
            Assert.AreEqual("Parent class non-static CTOR", messages.ElementAt(2), "Execution order is other than expected");
            Assert.AreEqual("Child class non-static CTOR", messages.ElementAt(3), "Execution order is other than expected");
        }
    }
}
