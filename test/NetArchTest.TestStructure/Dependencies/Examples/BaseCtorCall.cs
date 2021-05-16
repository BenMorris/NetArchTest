namespace NetArchTest.TestStructure.Dependencies.Examples
{
    public struct Id
    {
    }

    public static class StaticType
    {
        public static readonly Id SomeId;
    }

    public abstract class BaseCtorCallBase
    {
#pragma warning disable 219
        protected BaseCtorCallBase(params Id[] ids)
        {
        }
#pragma warning restore 219
    }

    public class BaseCtorCall : BaseCtorCallBase
    {
        public BaseCtorCall() :
            base(StaticType.SomeId)
        {
        }
    }
}
