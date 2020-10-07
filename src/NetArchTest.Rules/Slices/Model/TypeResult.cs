namespace NetArchTest.Rules.Slices.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Mono.Cecil;

    internal sealed class TypeResult : IFailingType
    {
        public TypeDefinition Type { get; set; }
        public bool IsPassing { get; set; }
        public string Reason { get; set; }

        TypeDefinition IFailingType.MonoTypeDefinition { get => Type; }
    }
}
