namespace NetArchTest.Rules.Slices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;
    using NetArchTest.Rules.Slices.Model;

    internal sealed class Slices : ISlices
    {
        private readonly IEnumerable<TypeDefinition> _types;

        public Slices(IEnumerable<TypeDefinition> types)
        {
            _types = types;
        }


        public ISliceList ByPrefix(string prefix)
        {
            var slicer = new Slicer();
            var slices = slicer.SliceByPrefix(_types, prefix);
            return new SliceList(slices);
        }
    }
}