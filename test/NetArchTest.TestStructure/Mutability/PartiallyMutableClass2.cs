namespace NetArchTest.TestStructure.Mutability
{
    /// <summary>
    /// An example class that has has some mutable members.
    /// </summary>
    public class PartiallyMutableClass2
    {
        public object Property {get; set;}

        public readonly object field;
    }
}