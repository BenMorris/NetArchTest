namespace NetArchTest.Rules.Slices.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal sealed class HaveDependenciesBetweenSlices : IFilter
    {
        public IEnumerable<TypeResult> Execute(SlicedTypes slicedTypes)
        {
            return null;
        }
    }
}
