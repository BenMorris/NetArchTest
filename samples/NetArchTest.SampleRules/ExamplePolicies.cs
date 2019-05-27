namespace NetArchTest.SampleRules
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
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

            Report(architecturePolicy.Evaluate(), Console.Out);

            var bogusPolicy = Policy.Define("Bogus Policy", "This policy demonstrates a failing policy")
                .For(Types.InCurrentDomain)
                .Add(t =>
                    t.That()
                    .AreInterfaces()
                    .Should()
                    .NotHaveNameStartingWith("I"),
                    "Crazy rule", "Interfaces should not start with an 'I'"
                );

            Report(bogusPolicy.Evaluate(), Console.Out);
        }

        /// <summary>
        /// Outputs a friendly display of the policy execution results;
        /// </summary>
        /// <param name="output"><see cref="TextWriter"/> for outputs</param>
        /// <returns><see cref="Task"/></returns>
        public static async Task ReportAsync(PolicyResults results, TextWriter output)
        {
            if (results.HasViolations)
            {
                await output.WriteLineAsync($"Policy violations found for: {results.Name}");

                foreach (var rule in results.Results)
                {
                    if (!rule.IsSuccessful)
                    {
                        await output.WriteLineAsync("-----------------------------------------------------------");
                        await output.WriteLineAsync($"Rule failed: {rule.Name}");

                        foreach (var type in rule.FailingTypes)
                        {
                            await output.WriteLineAsync($"\t {type.FullName}");
                        }
                    }
                }

                await output.WriteLineAsync("-----------------------------------------------------------");
            }
            else
            {
                await output.WriteLineAsync($"No policy violations found for: {results.Name}");
            }
        }

        /// <summary>
        /// A synchronous variant of <see cref="ReportAsync(TextWriter)"/>
        /// </summary>
        /// <param name="output"></param>
        public static void Report(PolicyResults results, TextWriter output) => ReportAsync(results, output).GetAwaiter().GetResult();
    }
}
