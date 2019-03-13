namespace NetArchTest.TestStructure.Mutability
{
    /// <summary>
    /// An example class that has has no mutable members.
    /// </summary>
    public class ImmutableClass
    {
        public object Property {get;}

        public readonly object field;
    }
}