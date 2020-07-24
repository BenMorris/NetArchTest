namespace NetArchTest.TestStructure.Dependencies.Examples
{
    /// <summary>
    /// An example class used in tests that identify nested dependencies.
    /// </summary>
    public class NestedDependencyTree
    {
        public class NestedLevel1
        {
            public class NestedLevel2
            {
                public class NestedDependency
                {

                }
            }

            public class NestedLevel2<T>
            {
                public class NestedDependency
                {

                }
            }

            public class NestedDependency
            {

            }
        }


        public class NestedLevel1<T>
        { 
            public class NestedDependency
            {

            }
        }
    }
}
