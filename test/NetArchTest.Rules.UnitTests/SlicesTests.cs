namespace NetArchTest.Rules.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NetArchTest.Rules.Slices;
    using NetArchTest.TestStructure.Dependencies.Examples;
    using Xunit;

    public class SlicesTests
    {
        [Fact(DisplayName = "Types are divided correctly into slices for a valid tree")]
        public void TypesAreDividedCorrectlyIntoSlicesForValidTree()
        {
            var slicedTypes = (Types.InAssembly(typeof(ExampleDependency).Assembly)
                               .Slice()
                               .ByNamespacePrefix("NetArchTest.TestStructure.Slices.ValidTree") as SliceList).GetSlicedTypes();

            Assert.Equal(9, slicedTypes.TypeCount);
            Assert.Equal(3, slicedTypes.Slices.Count());
            Assert.Equal(5, slicedTypes.Slices.Where(x => x.Name == "NetArchTest.TestStructure.Slices.ValidTree.FeatureA").First().Types.Count());
            Assert.Equal(3, slicedTypes.Slices.Where(x => x.Name == "NetArchTest.TestStructure.Slices.ValidTree.FeatureB").First().Types.Count());
            Assert.Single(slicedTypes.Slices.Where(x => x.Name == "NetArchTest.TestStructure.Slices.ValidTree.FeatureC").First().Types);
        }


    }
}
