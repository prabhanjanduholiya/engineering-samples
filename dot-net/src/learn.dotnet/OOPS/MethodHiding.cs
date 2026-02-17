namespace Learn.DotNet.OOPS.MethodHiding
{
    public class BaseClass
    {
        public string GetClassName()
        {
            return "BaseClass";
        }
    }

    public class ChildClassWithNewKeyword : BaseClass
    {
        public new string GetClassName()
        {
            return "ChildClass";
        }
    }
}
