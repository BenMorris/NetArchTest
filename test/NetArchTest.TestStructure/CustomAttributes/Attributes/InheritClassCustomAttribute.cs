namespace NetArchTest.TestStructure.CustomAttributes
{
    using System;

    /// <summary>
    /// Example class level custom attribute that inherits form other custom attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class InheritClassCustomAttribute : ClassCustomAttribute
    {
    }
}
