namespace NetArchTest.Rules.Slices.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Mono.Cecil;

    internal sealed class Slice
    {
        public string Name { get;  }
        public IEnumerable<TypeDefinition> Types { get;  }


        public Slice(string sliceName, List<TypeDefinition> types)
        {
            Name = sliceName;
            Types = types;
        }
    }
}
