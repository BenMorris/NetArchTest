namespace NetArchTest.TestStructure.FalsePositives
{
    public class ClassMatchingExample
    {
        private readonly NamespaceMatch.PatternMatchToo dependency;

        public ClassMatchingExample() 
        {
            dependency = new NamespaceMatch.PatternMatchToo();
        }
    }
}
