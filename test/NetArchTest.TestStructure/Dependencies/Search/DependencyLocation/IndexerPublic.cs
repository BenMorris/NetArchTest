namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a public indexer definition.    
    /// </summary>
    public class IndexerPublic
    { 
        public ExampleDependency this[int i]
        {
            get { return null; }
            set {  }
        }
    }
}
