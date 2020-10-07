using System;
using System.Collections.Generic;
using System.Text;

namespace NetArchTest.Rules.Slices.Model
{
    internal sealed class  Slicer
    {
        public SlicedTypes SliceByPrefix(IEnumerable<Mono.Cecil.TypeDefinition> _types, string prefix)
        {
            return new SlicedTypes();
        }
    }
}
