namespace Learn.DotNet.Design.Patterns.Creational.Builder
{
    public class Director
    {
        IHouseBuilder builder;

        public Director(IHouseBuilder houseBuilder)
        {
            builder = houseBuilder;
        }

        public House GetConstructedHouse()
        {
            return this.builder.GetHouse();
        }

        public void ConstructHouse()
        {
            this.builder.SetHouseNumber(10);
            this.builder.CreateFoundation();
            this.builder.CreateWalls();
            this.builder.CreateRoof();
            this.builder.DoElectricityWork();
            this.builder.DoPlatser();
            this.builder.DoPaint("White");
        }
    }
}
