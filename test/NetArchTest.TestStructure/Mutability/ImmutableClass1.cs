namespace NetArchTest.TestStructure.Mutability
{
    /// <summary>
    /// An example class that has has no mutable members.
    /// </summary>
    public class ImmutableClass1
    {
        public object GetOnlyProperty {get;}

        public readonly object readonlyField;
    }
}