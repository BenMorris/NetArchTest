namespace NetArchTest.TestStructure.FalsePositives
{
    public class ImplementationExample
    {
        private NamespaceMatchToo.ExampleClass dependency;

        public ImplementationExample() 
        {
            dependency = new NamespaceMatchToo.ExampleClass();
        }
    }
}
