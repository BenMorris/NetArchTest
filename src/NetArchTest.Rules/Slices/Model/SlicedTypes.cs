namespace NetArchTest.Rules.Slices.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal sealed class SlicedTypes
    {
        public int TypeCount { get; }       

        public IReadOnlyList<Slice> Slices { get; }


        public SlicedTypes(int typeCount, List<Slice> slices)
        {
            TypeCount = typeCount;
            Slices = slices;
        }
    }
}
