namespace Learn.DotNet.Design.Patterns.Creational.Builder
{
    public interface IHouseBuilder
    {
        public House GetHouse();

        #region Piecewise construction of house

        public void SetHouseNumber(int houseNumber);

        public void CreateFoundation();

        public void CreateWalls();

        public void CreateRoof();

        public void DoPlatser();

        public void DoElectricityWork();

        public void DoPaint(string colour);

        #endregion
    }
}
