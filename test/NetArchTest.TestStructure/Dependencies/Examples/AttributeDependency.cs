using System;

namespace NetArchTest.TestStructure.Dependencies.Examples
{
    /// <summary>
    /// An example attribute used in tests that identify dependencies.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    class AttributeDependency :  Attribute
    {
        public AttributeDependency()
        {

        }
        public AttributeDependency(object obj)
        {

        }
    }
}
