namespace Learn.DotNet.OOPS.MethodOverriding
{
    public class BaseClass
    {
        public virtual string GetClassName()
        {
            return "BaseClass";
        }
    }

    public class ChildClassWithOverrideKeyword : BaseClass
    {
        public override string GetClassName()
        {
            return "ChildClass";
        }
    }
}
