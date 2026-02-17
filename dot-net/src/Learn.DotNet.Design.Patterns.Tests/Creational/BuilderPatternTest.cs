using Learn.DotNet.Design.Patterns.Creational.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Learn.DotNet.Design.Patterns.Tests.Creational
{
    [TestClass]
    public class BuilderPatternTest
    {
        [TestMethod]
        public void VerifyHouseCreatedSuccessfullyUsingBuilderPattern()
        {
            IHouseBuilder houseBuilder = new ConcreteHouseBuilder();
            Director director = new Director(houseBuilder);
            director.ConstructHouse();
            House house = director.GetConstructedHouse();

            Assert.AreEqual("White", house.Paint, "Verify paint is done successfully");
            Assert.AreEqual(10, house.HouseNumber, "Verify house number is set successfully");
        }
    }
}
