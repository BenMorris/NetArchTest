namespace NetArchTest.SampleRules
{
    using NetArchTest.Rules;
    using NetArchTest.SampleLibrary.Data;
    using NetArchTest.SampleLibrary.Services;

    /// <summary>
    /// Examples of rule definitions.
    /// </summary>
    public static class ExampleRules
    {
        public static void Run()
        {
            //****************************************************
            // Enforcing layered architecture
 
            // Controllers should not directly reference repositories

            var result = Types.InCurrentDomain()
                .That()
                .ResideInNamespace("NetArchTest.SampleLibrary.Presentation")
                .ShouldNot()
                .HaveDependencyOn("NetArchTest.SampleLibrary.Data")
                .GetResult().IsSuccessful;

            //****************************************************
            // Controlling external dependencies
            
            // Only classes in the data namespace can have a dependency on System.Data

            result = Types.InCurrentDomain()
                .That().HaveDependencyOn("System.Data")
                .And().ResideInNamespace(("ArchTest"))
                .Should().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                .GetResult().IsSuccessful;


            //****************************************************
            // Miscellaneous application-specific rules

            // All the classes in the data namespace should implement IRepository
            result = Types.InCurrentDomain()
                .That().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                .And().AreClasses()
                .Should().ImplementInterface(typeof(IRepository<>))
                .GetResult().IsSuccessful;

            // Classes that implement IRepository should have the suffix "Repository"
            result = Types.InCurrentDomain()
                .That().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                .And().AreClasses()
                .Should().HaveNameEndingWith("Repository")
                .GetResult().IsSuccessful;

            // Classes that implement IRepository must reside in the Data namespace
            result = Types.InCurrentDomain()
                .That().ImplementInterface(typeof(IRepository<>))
                .Should().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                .GetResult().IsSuccessful;

            // All the service classes should be sealed
            result = Types.InCurrentDomain()
                .That().ImplementInterface(typeof(IWidgetService))
                .Should().BeSealed()
                .GetResult().IsSuccessful;

            //****************************************************
            // Generic implementation rules

            // Interface names should start with an "I"
            result = Types.InCurrentDomain()
                .That().AreInterfaces()
                .Should()
                .HaveNameStartingWith("I")
                .GetResult().IsSuccessful;

            //****************************************************
            // Custom rule example

            var myRule = new CustomRule();

            // Write your own custom rules that can be used as both predicates and conditions
            result = Types.InCurrentDomain()
                .That().AreClasses()
                .Should()
                .MeetCustomRule(myRule)
                .GetResult().IsSuccessful;
        }
    }
}
