using NetArchTest.Rules.Matches;

namespace NetArchTest.SampleRules
{
    using NetArchTest.Rules;
    using NetArchTest.SampleLibrary.Data;
    using NetArchTest.SampleLibrary.Services;

    class Program
    {
        static void Main(string[] args)
        {
            //****************************************************
            // Enforcing layered architecture

            // Controllers should not directly reference repositories
            var result = Types.InCurrentDomain()
                .That()
                .ResideInNamespace("NetArchTest.SampleLibrary.Presentation")
                .ShouldNot()
                .HaveDependencyOn("NetArchTest.SampleLibrary.Data")
                .GetResult();

            //****************************************************
            // Controlling external dependencies

            // Only classes in the data namespace can have a dependency on System.Data
            result = Types.InCurrentDomain()
                .That().HaveDependencyOn(Globbing.New("System.Data"))
                .And().ResideInNamespace(("ArchTest"))
                .Should().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                .GetResult();

            //****************************************************
            // Miscellaneous application-specific rules

            // All the classes in the data namespace should implement IRepository
            result = Types.InCurrentDomain()
                .That().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                .And().AreClasses()
                .Should().ImplementInterface(typeof(IRepository<>))
                .GetResult();

            // Classes that implement IRepository should have the suffix "Repository"
            result = Types.InCurrentDomain()
                .That().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                .And().AreClasses()
                .Should().HaveNameEndingWith("Repository")
                .GetResult();

            // Classes that implement IRepository must reside in the Data namespace
            result = Types.InCurrentDomain()
                .That().ImplementInterface(typeof(IRepository<>))
                .Should().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                .GetResult();

            // All the service classes should be sealed
            result = Types.InCurrentDomain()
                .That().ImplementInterface(typeof(IWidgetService))
                .Should().BeSealed()
                .GetResult();

            //****************************************************
            // Generic implementation rules

            // Interface names should start with an "I"
            result = Types.InCurrentDomain()
                .That().AreInterfaces()
                .Should()
                .HaveNameStartingWith("I")
                .GetResult();
        }
    }
}
