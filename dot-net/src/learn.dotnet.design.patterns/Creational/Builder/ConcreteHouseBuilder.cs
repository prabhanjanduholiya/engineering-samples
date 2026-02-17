namespace Learn.DotNet.Design.Patterns.Creational.Builder
{
    public class ConcreteHouseBuilder : IHouseBuilder
    {
        House house;

        public ConcreteHouseBuilder()
        {
            house = new House();
        }

        public House GetHouse()
        {
            return house;
        }

        #region Piecewise construction of house

        public void CreateFoundation()
        {
            house.FoundationCreated = true;
        }

        public void CreateRoof()
        {
            house.RoofCreated = true;
        }

        public void CreateWalls()
        {
            house.WallsCreated = true;
        }

        public void DoElectricityWork()
        {
            house.ElectricityWorkDone = true;
        }

        public void DoPaint(string colour)
        {
            house.Paint = colour;
        }

        public void DoPlatser()
        {
            house.PlasterCreated = true;
        }

        public void SetHouseNumber(int houseNumber)
        {
            house.HouseNumber = houseNumber;
        }
        #endregion
    }
}
