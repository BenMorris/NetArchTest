namespace NetArchTest.Rules.Slices
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Mono.Cecil;
    using NetArchTest.Rules.Extensions;

    [DebuggerDisplay("{TypeName}")]
    internal sealed class FailingType : IFailingType
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TypeDefinition _monoTypeDefinition;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Type> _type;

        public FailingType(TypeDefinition monoTypeDefinition)
        {
            _monoTypeDefinition = monoTypeDefinition;
            _type = new Lazy<Type>(() =>
            {
                Type type = null;
                try
                {
                    type = _monoTypeDefinition.ToType();
                }
                catch { }
                return type;
            });
        }


        public Type Type { get => _type.Value; }
        public string TypeName { get => _monoTypeDefinition.FullName; }
    }
}
