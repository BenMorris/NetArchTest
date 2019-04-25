namespace NetArchTest.SampleRules
{
    using NetArchTest.Rules;
    using NetArchTest.Rules.Policies;
    using NetArchTest.SampleLibrary.Data;
    using NetArchTest.SampleLibrary.Services;

    /// <summary>
    /// Examples of how rules can be aggregated into policies that are executed in a group.
    /// </summary>
    public class ExamplePolicies
    {
        public static void Run()
        {
            var architecturePolicy = Policy.Define("Passing Policy", "This policy demonstrated a valid passing policy with reasonable rules")
                .For(Types.InCurrentDomain)
                .Add(t =>
                   t.That()
                   .ResideInNamespace("NetArchTest.SampleLibrary.Presentation")
                   .ShouldNot()
                   .HaveDependencyOn("NetArchTest.SampleLibrary.Data"),
                   "Enforcing layered architecture", "Controllers should not directly reference repositories"
                )
                .Add(t =>
                    t.That().HaveDependencyOn("System.Data")
                    .And().ResideInNamespace(("ArchTest"))
                    .Should().ResideInNamespace("NetArchTest.SampleLibrary.Data"),
                    "Controlling external dependencies", "Only classes in the data namespace can have a dependency on System.Data"
                )
                .Add(t =>
                    t.That().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                    .And().AreClasses()
                    .Should().ImplementInterface(typeof(IRepository<>)),
                    "Miscellaneous application-specific rules", "All the classes in the data namespace should implement IRepository"
                )
                .Add(t =>
                    t.That().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                    .And().AreClasses()
                    .Should().HaveNameEndingWith("Repository"),
                    "Miscellaneous application-specific rules", "Classes that implement IRepository should have the suffix 'Repository'"
                )
                .Add(t =>
                    t.That().ImplementInterface(typeof(IRepository<>))
                    .Should().ResideInNamespace("NetArchTest.SampleLibrary.Data"),
                    "Miscellaneous application-specific rules", "Classes that implement IRepository must reside in the Data namespace"
                )
                .Add(t =>
                    t.That().ImplementInterface(typeof(IWidgetService))
                    .Should().BeSealed(),
                    "Miscellaneous application-specific rules", "All the service classes should be sealed")
                .Add(t =>
                    t.That()
                    .AreInterfaces()
                    .Should()
                    .HaveNameStartingWith("I"),
                    "Generic implementation rules", "Interface names should start with an 'I'"
                );

            var results = architecturePolicy.Evaluate();

            var bogusPolicy = Policy.Define("Bogus Policy", "This policy demonstrates a failing policy")
                .For(Types.InCurrentDomain)
                .Add(t =>
                    t.That()
                    .AreInterfaces()
                    .Should()
                    .NotHaveNameStartingWith("I"),
                    "Crazy rules", "Interfaces should not start with an 'I'"
                );

            var bogusResults = bogusPolicy.Evaluate();
        }
    }
}
