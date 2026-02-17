using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Learn.DotNet.OOPS.Tests
{

    [TestClass]
    public class MethodHidingVsOverridingTest
    {
        [TestMethod]
        public void MethodHidingTest()
        {
            Learn.DotNet.OOPS.MethodHiding.BaseClass baseClass = new Learn.DotNet.OOPS.MethodHiding.BaseClass();
            Assert.AreEqual("BaseClass", baseClass.GetClassName(), "Base class instance assigned to base class type variable");

            Learn.DotNet.OOPS.MethodHiding.ChildClassWithNewKeyword childClass = new Learn.DotNet.OOPS.MethodHiding.ChildClassWithNewKeyword();
            Assert.AreEqual("ChildClass", childClass.GetClassName(), "Child class instance assigned to child class type variable");

            Learn.DotNet.OOPS.MethodHiding.BaseClass childClassInstance = new Learn.DotNet.OOPS.MethodHiding.ChildClassWithNewKeyword();
            Assert.AreEqual("BaseClass", childClassInstance.GetClassName(), "Child class instance assigned to base class type variable");
        }

        [TestMethod]
        public void MethodOverridingTest()
        {
            Learn.DotNet.OOPS.MethodOverriding.BaseClass baseClass = new Learn.DotNet.OOPS.MethodOverriding.BaseClass();
            Assert.AreEqual("BaseClass", baseClass.GetClassName(), "Base class instance assigned to base class type variable");

            Learn.DotNet.OOPS.MethodOverriding.ChildClassWithOverrideKeyword childClass = new MethodOverriding.ChildClassWithOverrideKeyword();
            Assert.AreEqual("ChildClass", childClass.GetClassName(), "Child class instance assigned to child class type variable");

            Learn.DotNet.OOPS.MethodOverriding.BaseClass childClassInstance = new Learn.DotNet.OOPS.MethodOverriding.ChildClassWithOverrideKeyword();
            Assert.AreEqual("ChildClass", childClassInstance.GetClassName(), "Child class instance assigned to base class type variable");
        }
    }


}
