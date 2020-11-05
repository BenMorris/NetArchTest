namespace NetArchTest.Rules.Slices.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mono.Cecil;
    using NetArchTest.Rules.Dependencies;

    internal sealed class HaveDependenciesBetweenSlices : IFilter
    {
        public IEnumerable<TypeResult> Execute(SlicedTypes slicedTypes)
        {
            var dependencySearch = new DependencySearch();
            var result = new List<TypeResult>(slicedTypes.TypeCount);

            for (int i = 0; i < slicedTypes.Slices.Count; i++)
            {
                var slice = slicedTypes.Slices[i];
                var dependencies = slicedTypes.Slices.Where((_, index) => index != i).Select(x => x.Name).ToList();

                var foundTypes =  dependencySearch.FindTypesThatHaveDependencyOnAny(slice.Types, dependencies);
                var lookup = new HashSet<TypeDefinition>(foundTypes);

                foreach (var type in slice.Types)
                {
                    bool isPassing = lookup.Contains(type);
                    var typeResult = new TypeResult(type, isPassing);                    
                    result.Add(typeResult);
                }
            }

            return result;
        }
    }
}