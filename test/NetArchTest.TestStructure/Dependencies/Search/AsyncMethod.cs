namespace NetArchTest.TestStructure.Dependencies.Search
{
    using System.Threading.Tasks;
    using NetArchTest.TestStructure.Dependencies;

    /// <summary>
    /// Example class that includes a dependency in an asynchronous method definition.    
    /// </summary>
    public class AsyncMethod
    {
        public async Task ExampleMethod()
        {
            var test = new ExampleDependency();
            await Task.CompletedTask;
        }
    }
}
