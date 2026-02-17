namespace Learn.DotNet.Design.Patterns.Creational.Builder
{
    public class House
    {
        public int HouseNumber { get; set; }

        public string Paint { get; set; }
        public bool FoundationCreated { get; internal set; }
        public bool RoofCreated { get; internal set; }
        public bool WallsCreated { get; internal set; }
        public bool ElectricityWorkDone { get; internal set; }
        public bool PlasterCreated { get; internal set; }
    }
}
