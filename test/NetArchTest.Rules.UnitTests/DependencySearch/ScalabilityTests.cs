namespace NetArchTest.Rules.UnitTests.DependencySearch
{
    using Mono.Cecil;
    using NetArchTest.Rules.Dependencies;
    using NetArchTest.TestStructure.Dependencies.Examples;
    using NetArchTest.TestStructure.Dependencies.Implementation;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// This test collection verifies that search methods take time which linearly depends
    /// 1) on count of dependencies to search for (initial indexing of dependencies) and
    /// 2) on total count of input type members,
    /// i.e. dependecy search algorithm is scalable.
    /// </summary>
    /// <remarks>
    /// Each search method is called twice and whole time is measured using the System.Diagnostics.StopWatch;
    /// then average time of one call is calculated. Such an iteration is repeated several times in order to
    /// collect the statistics which is processed at the end producing mean value and its margins
    /// for 95% confidence interval.
    /// 
    /// Those measurements are done for input sets of three sizes: small, medium and large.
    /// The results allow to calculate linearity factor k of the hypothesis t=k*n+c,
    /// where n - size of input, t - the time ellapsed, c - some constant.
    /// If two calculated mean values of k differ not more than the sum of their margins,
    /// then the time is considered to depend linearly on the size of input set.
    /// 
    /// For more precise and more reliable measurenents and calculations without outliers
    /// there could be used specialized frameworks like Benchmark.Net, however it could increase
    /// test execution time from seconds to minutes.
    /// </remarks>
    [CollectionDefinition("Dependency Search Scalability", DisableParallelization = true)]
    public class ScalabilityTests
    {
        // Type definitions input sets of different size
        private static IList<TypeDefinition> _inputTypesSmallSet;
        private static IList<TypeDefinition> _inputTypesMediumSet;
        private static IList<TypeDefinition> _inputTypesLargeSet;

        // Total number of members of all types from input sets.
        private static int _inputMembersSmallSetCount;
        private static int _inputMembersMediumSetCount;
        private static int _inputMembersLargeSetCount;

        // Dependency sets of different size; each dependency is referenced by 
        // at least one member of a type from input set
        private static List<string> _dependenciesToSearchSmallSet;
        private static List<string> _dependenciesToSearchMediumSet;
        private static List<string> _dependenciesToSearchLargeSet;

        // Additional items to sets of dependencies to search for; none of them is referenced,
        // they only increase each corresponding set.
        private static List<string> _nonDependenciesToSearchSmallSet;
        private static List<string> _nonDependenciesToSearchMediumSet;
        private static List<string> _nonDependenciesToSearchLargeSet;

        private static string _implementationNamespace = typeof(HasDependency).Namespace;
        private static string _dependencyNamespace = typeof(ExampleDependency).Namespace;

        static ScalabilityTests()
        {
            _dependenciesToSearchSmallSet = ConstructDependencyToSearchList(0x40, "ExampleDependency");     // 64 items
            _dependenciesToSearchMediumSet = ConstructDependencyToSearchList(0x80, "ExampleDependency");    // 128 items
            _dependenciesToSearchLargeSet = ConstructDependencyToSearchList(0x100, "ExampleDependency");    // 256 items

            _nonDependenciesToSearchSmallSet = ConstructDependencyToSearchList(0x40, "NonDependency");      // 64 items
            _nonDependenciesToSearchMediumSet = ConstructDependencyToSearchList(0x80, "NonDependency");     // 128 items
            _nonDependenciesToSearchLargeSet = ConstructDependencyToSearchList(0x100, "NonDependency");     // 256 items

            _inputTypesSmallSet = StubTypeDefinitionWithDependencies(0x80, _dependenciesToSearchSmallSet, out _inputMembersSmallSetCount);      // 128 types
            _inputTypesMediumSet = StubTypeDefinitionWithDependencies(0x100, _dependenciesToSearchMediumSet, out _inputMembersMediumSetCount);  // 256 types
            _inputTypesLargeSet = StubTypeDefinitionWithDependencies(0x200, _dependenciesToSearchLargeSet, out _inputMembersLargeSetCount);     // 512 types
        }

        [Fact(DisplayName = "Time to search all dependencies ~ O(n+m), n - total number of members in all input types; m - number of dependencies to search",
              Timeout = 60000)]
        public async void FindTypesWithAllDependencies_TimeGrowsLinearly()
        {
            // Arrange
            var runner = new Runner(iterationCount: 9, repeatCount: 2);

            // Warm up
            await runner.Run(() => FindTypesWithAllDependencies(_inputTypesLargeSet, _dependenciesToSearchLargeSet));

            // Act, measure the time spent on small set
            var ellapsed0 = await runner.Run(() => FindTypesWithAllDependencies(_inputTypesSmallSet, _dependenciesToSearchSmallSet));

            // Act, measure the time spent on medium set
            var ellapsed1 = await runner.Run(() => FindTypesWithAllDependencies(_inputTypesMediumSet, _dependenciesToSearchMediumSet));

            // Act, measure the time spent on large set
            var ellapsed2 = await runner.Run(() => FindTypesWithAllDependencies(_inputTypesLargeSet, _dependenciesToSearchLargeSet));

            double n0 = _inputMembersSmallSetCount + _dependenciesToSearchSmallSet.Count;
            double n1 = _inputMembersMediumSetCount + _dependenciesToSearchMediumSet.Count;
            double n2 = _inputMembersLargeSetCount + _dependenciesToSearchLargeSet.Count;

            // Assert hypothesis: t(n)=k*n+c
            // t0 = k*n0 + c |
            // t1 = k*n1 + c | ==> k=(t1-t0)/(n1-n0)=?=(t2-t1)/(n2-n1)
            // t2 = k*n2 + c |
            double k1 = (ellapsed1.Mean - ellapsed0.Mean) / (n1 - n0);
            double k2 = (ellapsed2.Mean - ellapsed1.Mean) / (n2 - n1);

            double dk1 = (ellapsed1.Margins + ellapsed0.Margins) / (n1 - n0);
            double dk2 = (ellapsed2.Margins + ellapsed1.Margins) / (n2 - n1);

            // Assert that confidence intervals of both k values do overlap,
            // i.e. both k values are considered to be equal.
            // k1-dk1      k1+dk1
            //   (.....k1    )
            //            (.....k2    )
            //          k2-dk2      k2+dk2
            Assert.True(Math.Abs(k1 - k2) <= dk1 + dk2,
                $"{k1}+-{dk1}; {k2}+-{dk2}\nSize; Ellapsed Ticks:\n{n0}; {ellapsed0.Mean}+-{ellapsed0.Margins}\n{n1}; {ellapsed1.Mean}+-{ellapsed1.Margins}\n{n2}; {ellapsed2.Mean}+-{ellapsed2.Margins}");
        }


        [Fact(DisplayName = "Time to search any dependency ~ O(n+m), n - total number of members in all input types; m - number of dependencies to search", 
            Timeout = 60000)]
        public async void FindTypesWithAnyDependencies_TimeGrowsLinearly()
        {
            // Arrange
            var toSearchSmallSet = _nonDependenciesToSearchSmallSet.Concat(_dependenciesToSearchSmallSet);
            var toSearchMediumSet = _nonDependenciesToSearchMediumSet.Concat(_dependenciesToSearchMediumSet);
            var toSearchLargeSet = _nonDependenciesToSearchLargeSet.Concat(_dependenciesToSearchLargeSet);

            var runner = new Runner(iterationCount: 9, repeatCount: 2);

            // Warm up
            await runner.Run(() => FindTypesWithAnyDependencies(_inputTypesLargeSet, toSearchLargeSet));

            // Act, measure the time spent on small set
            var ellapsed0 = await runner.Run(() => FindTypesWithAnyDependencies(_inputTypesSmallSet, toSearchSmallSet));

            // Act, measure the time spent on medium set
            var ellapsed1 = await runner.Run(() => FindTypesWithAnyDependencies(_inputTypesMediumSet, toSearchMediumSet));

            // Act, measure the time spent on large set
            var ellapsed2 = await runner.Run(() => FindTypesWithAnyDependencies(_inputTypesLargeSet, toSearchLargeSet));

            // Assert hypothesis: t(n)=k*n+c
            // t0 = k*n0 + c |
            // t1 = k*n1 + c | ==> k=(t1-t0)/(n1-n0)=?=(t2-t1)/(n2-n1)
            // t2 = k*n2 + c |

            int n0 = _inputMembersSmallSetCount + _nonDependenciesToSearchSmallSet.Count + _dependenciesToSearchSmallSet.Count;
            int n1 = _inputMembersMediumSetCount + _nonDependenciesToSearchMediumSet.Count + _dependenciesToSearchMediumSet.Count;
            int n2 = _inputMembersLargeSetCount + _nonDependenciesToSearchLargeSet.Count + _dependenciesToSearchLargeSet.Count;

            double k1 = (ellapsed1.Mean - ellapsed0.Mean) / (n1 - n0);
            double k2 = (ellapsed2.Mean - ellapsed1.Mean) / (n2 - n1);

            double dk1 = (ellapsed1.Margins + ellapsed0.Margins) / (n1 - n0);
            double dk2 = (ellapsed2.Margins + ellapsed1.Margins) / (n2 - n1);

            // Assert that confidence intervals of both k values do overlap,
            // i.e. both k values are considered to be equal.
            // k1-dk1      k1+dk1
            //   (.....k1    )
            //            (.....k2    )
            //          k2-dk2      k2+dk2
            Assert.True(Math.Abs(k1 - k2) <= dk1 + dk2,
                $"{k1}+-{dk1}; {k2}+-{dk2}\nSize; Ellapsed Ticks:\n{n0}; {ellapsed0.Mean}+-{ellapsed0.Margins}\n{n1}; {ellapsed1.Mean}+-{ellapsed1.Margins}\n{n2}; {ellapsed2.Mean}+-{ellapsed2.Margins}");
        }

        private void FindTypesWithAnyDependencies(IEnumerable<TypeDefinition> inputTypes, IEnumerable<string> dependenciesToSearch)
        {
            var search = new DependencySearch();

            // Act
            var result = search.FindTypesThatHaveDependencyOnAny(inputTypes, dependenciesToSearch);

            Assert.Equal(inputTypes.Count(), result.Count());
        }

        private void FindTypesWithAllDependencies(IEnumerable<TypeDefinition> inputTypes, IEnumerable<string> dependenciesToSearch)
        {
            // Arrange
            var search = new DependencySearch();

            // Act
            var result = search.FindTypesThatHaveDependencyOnAll(inputTypes, dependenciesToSearch);

            // Assert
            Assert.Equal(inputTypes.Count(), result.Count());
        }

        /// <summary>
        /// Builds the list of type full names formed like "NetArchTest.TestStructure.Dependencies.Examples.<nameBase>0/nameBase>>"
        /// </summary>
        /// <param name="dependenciesCount">Number of full names to generate</param>
        /// <param name="nameBase">Base part of the name to be extended with item's index like <nameBase>0, <nameBase>1,...<nameBase>n</param>
        /// <returns>List of type full names.</returns>
        static private List<string> ConstructDependencyToSearchList(int dependenciesCount, string nameBase)
        {
            return Enumerable.Range(0, dependenciesCount)
                .Select(x => string.Concat(_dependencyNamespace, ".", nameBase, x.ToString(CultureInfo.InvariantCulture)))
                .ToList();
        }

        /// <summary>
        /// Generates type definitions with properties; each property references one type from given list of dependencies,
        /// so that every type references all types from that list.
        /// </summary>
        /// <param name="typesCount">Number of type definitions to generate</param>
        /// <param name="dependenciesFullNames">List of dependencies for each type to reference</param>
        /// <param name="totalCountOfMembers">Returns total number of members in all generated type definitions</param>
        /// <returns>List of type definitions, each of them refers to all dependencies from given list</returns>
        static private IList<TypeDefinition> StubTypeDefinitionWithDependencies(int typesCount, IReadOnlyCollection<string> dependenciesFullNames,
            out int totalCountOfMembers)
        {
            var types = new List<TypeDefinition>(typesCount);

            for (int i = typesCount; i-- != 0;)
            {
                var typeDefinition = new TypeDefinition(_implementationNamespace, string.Concat("InputType", i.ToString(CultureInfo.InvariantCulture)),
                    TypeAttributes.UnicodeClass | TypeAttributes.Public);

                int j = 0;
                foreach (var fullName in dependenciesFullNames)
                {
                    int nameStart = fullName.LastIndexOf('.') + 1;
                    string dependencyNamespace = nameStart != 0 ? fullName.Substring(0, nameStart - 1) : string.Empty;
                    string dependencyName = fullName.Substring(nameStart);

                    var dependency = new TypeDefinition(dependencyNamespace, dependencyName, TypeAttributes.Abstract);

                    typeDefinition.Properties.Add(new PropertyDefinition(string.Concat("Property", j++.ToString(CultureInfo.InvariantCulture)),
                        PropertyAttributes.None, dependency));
                }

                types.Add(typeDefinition);
            }

            totalCountOfMembers = typesCount * dependenciesFullNames.Count;
            return types;
        }

        /// <summary>
        /// Runner for single benchmark.
        /// </summary>
        private sealed class Runner
        {
            private readonly int _iterationCount;
            private readonly int _repeatCount;

            /// <summary>
            /// Initializes the runner.
            /// </summary>
            /// <param name="iterationCount">Count of iterations to calculate mean value of ellapsed time and its margins</param>
            /// <param name="repeatCount">Count of runs of a benchmark per each iteration to calculate average value of ellapsed time</param>
            public Runner(int iterationCount = 16, int repeatCount = 5)
            {
                _iterationCount = iterationCount;
                _repeatCount = repeatCount;
            }

            /// <summary>
            /// Performs the benchmark several times and measures ellapsed time.
            /// </summary>
            /// <param name="action">Benchmark action method to perform</param>
            /// <returns>Ellapsed time mean value and its margins for 95% confidence interval</returns>
            public Task<Statistics.Value> Run(Action action)
            {
                return Task.Run(() => MeasureEllapsedTime(action));
            }

            private Statistics.Value MeasureEllapsedTime(Action action)
            {
                var ellapsed = new double[_iterationCount];

                var threadOldPriority = System.Threading.Thread.CurrentThread.Priority;
                System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.AboveNormal;

                try
                {
                    for (int i = 0; i != _iterationCount; i++)
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();

                        for (int j = _repeatCount; j-- != 0;)
                        {
                            action();
                        }

                        stopwatch.Stop();
                        ellapsed[i] = stopwatch.ElapsedTicks / (double)_repeatCount;
                    }
                }
                finally
                {
                    System.Threading.Thread.CurrentThread.Priority = threadOldPriority;
                }

                return Statistics.CalculateValue(ellapsed);
            }
        }

        /// <summary>
        /// Methods to process benchmark run results
        /// </summary>
        private static class Statistics
        {
            /// <summary>
            /// Represents mean value together with its margins
            /// </summary>
            public struct Value
            {
                public double Mean
                {
                    get;
                }
                public double Margins
                {
                    get;
                }

                public Value(double mean, double margins)
                {
                    Mean = mean;
                    Margins = margins;
                }
            }

            /// <summary>
            /// Calculates mean value and its margins for 95% confidence interval
            /// </summary>
            /// <param name="results">Results of several runs of the same benchmark</param>
            /// <returns>Resulting mean value and its margins for 95% confidence interval</returns>
            public static Value CalculateValue(IReadOnlyCollection<double> results)
            {
                if (results.Count < 2)
                {
                    throw new ArgumentException("There must be at least two results", nameof(results));
                }

                double mean = results.Average();
                double deviation = Math.Sqrt(results.Select(x => (x - mean) * (x - mean)).Sum() / (results.Count - 1) / results.Count);

                return new Value(mean: mean, margins: deviation * GetTinv95(results.Count));
            }

            // Student factors for the confidential probability 0.95 and different numbers of degrees of freedom.
            private static double GetTinv95(int n)
            {
                return (n > 10 && n < 15) ? 2.2
                    : (n >= 15 && n < 28) ? 2.1
                    : n == 2 ? 12.7
                    : n == 3 ? 4.3
                    : n == 4 ? 3.2
                    : n == 5 ? 2.8
                    : n == 6 ? 2.6
                    : (n == 7 || n == 8) ? 2.4
                    : (n == 9 || n == 10) ? 2.3
                    : 2.0;
            }
        }
    }
}
