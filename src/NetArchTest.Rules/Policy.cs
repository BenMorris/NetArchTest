using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetArchTest.Rules
{
    /// <summary>
    /// A simple aggregate of rules and results for overall reporting
    /// </summary>
    public sealed class Policy
    {
        private Policy()
        {
        }

        /// <summary>
        /// A detailed description of the policy
        /// </summary>
        public string Description { get; private set; }

        private Types _types;
        private Func<Types> _typesLocator;
        private bool _hasEvaluated;

        /// <summary>
        /// A lazy executed method to return the types for the evaluators
        /// </summary>
        /// <param name="typesLocator"><see cref="Types"/>></param>
        /// <returns></returns>
        public Policy For(Func<Types> typesLocator)
        {
            _typesLocator = typesLocator;
            return this;
        }

        private List<Func<Types, TestResult>> TestResultFuncs = new List<Func<Types, TestResult>>();

        /// <summary>
        /// The results from the checks executed in the policy.
        /// </summary>
        public List<TestResult> TestResults = new List<TestResult>();

        /// <summary>
        /// The simple name of the policy
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Does the policy have any rule violations
        /// </summary>
        public bool HasVoilations
        {
            get
            {
                if (!_hasEvaluated)
                    Evaluate();
                return TestResults.Any(r => !r.IsSuccessful);
            }
        }

        /// <summary>
        /// Defines a policy for use in the fluent syntax
        /// </summary>
        /// <param name="name"><see cref="Policy.Name"/></param>
        /// <param name="description"><see cref="Policy.Description"/></param>
        /// <returns></returns>
        public static Policy Define(string name, string description)
        {
            return new Policy
            {
                Name = name,
                Description = description
            };
        }

        /// <summary>
        /// Adds a Func that evaluates to a <see cref="TestResult"/>. Note: Be sure to use mark the TestResult with a Rule using <see cref="TestResult.MarkForRule(string, string, int?)"/>
        /// </summary>
        /// <param name="testResult">A result of an evaulation</param>
        /// <returns><see cref="Policy"/> for use in Fluent syntax</returns>
        public Policy Add(Func<Types, TestResult> testResult)
        {
            testResult.Invoke(Types.StubTypes());
            if (_hasEvaluated)
                throw new InvalidOperationException("This policy has already executed. Please only add rules before execution");
            TestResultFuncs.Add(testResult);
            return this;
        }

        /// <summary>
        /// Outputs a friendly display of the policy execution results;
        /// </summary>
        /// <param name="output"><see cref="TextWriter"/> for outputs</param>
        /// <returns><see cref="Task"/></returns>
        public async Task ReportAsync(TextWriter output)
        {
            if (HasVoilations)
            {
                var violations = TestResults.Where(x => !x.IsSuccessful);
                var aggregate = violations.GroupBy(x =>
                $"{(x.Rule.Id.HasValue ? x.Rule.Id.Value.ToString() : x.Rule.Name)} - {x.Rule.Description}");

                await output.WriteLineAsync($"---- Policy Results for Types {_types.Description}");

                foreach (var r in aggregate)
                {
                    await output.WriteLineAsync($"\t Rule [{r.Key}] Voilations");
                    foreach (var rule in r.Where(x => !x.IsSuccessful).SelectMany(x => x.FailingTypes).Distinct())
                    {
                        await output.WriteLineAsync($"\t\t [{rule}]");
                    }
                }
                await output.WriteLineAsync("-----------------------------------------------------------");
                await output.WriteLineAsync();
                await output.WriteLineAsync();
            }
        }

        /// <summary>
        /// A synchronous variant of <see cref="ReportAsync(TextWriter)"/>
        /// </summary>
        /// <param name="output"></param>
        public void Report(TextWriter output) => ReportAsync(output).GetAwaiter().GetResult();

        /// <summary>
        /// Evaluates all the rules added againsts the <see cref="Types" assigned/>
        /// </summary>
        public void Evaluate()
        {
            if (_hasEvaluated)
                return;

            if (_typesLocator == null)
            {
                throw new InvalidOperationException("You must call the .For() methods before executing to point to the types to evaluate");
            }

            _types = _typesLocator();
            TestResults.AddRange(TestResultFuncs.Select(t => t(_types)));
            _hasEvaluated = true;
        }
    }
}