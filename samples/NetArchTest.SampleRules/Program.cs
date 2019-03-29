namespace NetArchTest.SampleRules
{
    using NetArchTest.Rules;
    using NetArchTest.SampleLibrary.Data;
    using NetArchTest.SampleLibrary.Services;
    using NetArchTest.Rules.Extensions;
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var architecturePolicy = Policy.Define("Passing Policy", "This policy demonstrated a valid passing policy with reasonable rules")
                .For(Types.InCurrentDomain)
                .Add(t =>
                   t.That()
                   .ResideInNamespace("NetArchTest.SampleLibrary.Presentation")
                   .ShouldNot()
                   .HaveDependencyOn("NetArchTest.SampleLibrary.Data")
                   .GetResult()
                   .MarkForRule("Enforcing layered architecture", "Controllers should not directly reference repositories")
                )
                .Add(t =>
                    t.That().HaveDependencyOn("System.Data")
                    .And().ResideInNamespace(("ArchTest"))
                    .Should().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                    .GetResult()
                    .MarkForRule("Controlling external dependencies", "Only classes in the data namespace can have a dependency on System.Data")
                )
                .Add(t =>
                    t.That().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                    .And().AreClasses()
                    .Should().ImplementInterface(typeof(IRepository<>))
                    .GetResult()
                    .MarkForRule("Miscellaneous application-specific rules", "All the classes in the data namespace should implement IRepository")
                )
                .Add(t =>
                    t.That().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                    .And().AreClasses()
                    .Should().HaveNameEndingWith("Repository")
                    .GetResult()
                    .MarkForRule("Miscellaneous application-specific rules", "Classes that implement IRepository should have the suffix 'Repository'")
                )
                .Add(t =>
                    t.That().ImplementInterface(typeof(IRepository<>))
                    .Should().ResideInNamespace(("NetArchTest.SampleLibrary.Data"))
                    .GetResult()
                    .MarkForRule("Miscellaneous application-specific rules", "Classes that implement IRepository must reside in the Data namespace")
                )
                .Add(t =>
                    t.That().ImplementInterface(typeof(IWidgetService))
                    .Should().BeSealed()
                    .GetResult()
                    .MarkForRule("Miscellaneous application-specific rules", "All the service classes should be sealed")
                )
                .Add(t =>
                    t.That()
                    .AreInterfaces()
                    .Should()
                    .HaveNameStartingWith("I")
                    .GetResult()
                    .MarkForRule("Generic implementation rules", "Interface names should start with an 'I'")
                );
            architecturePolicy.Evaluate();

            architecturePolicy.Report(Console.Out);

            var bogusPolicy = Policy.Define("Bogus Policy", "This policy demonstrates a failing policy")
                .For(Types.InCurrentDomain)
                .Add(t =>
                    t.That()
                    .AreInterfaces()
                    .Should()
                    .NotHaveNameStartingWith("I")
                    .GetResult()
                    .MarkForRule("Crazy rules", "Interfaces should not start with an 'I'")
                );
            bogusPolicy.Evaluate();

            bogusPolicy.Report(Console.Out);
        }
    }
}