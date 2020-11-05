namespace NetArchTest.Rules.Slices.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Mono.Cecil;

    internal sealed class TypeResult : IFailingType
    { 
        public TypeDefinition Type { get;  }
        public bool IsPassing { get;  }
        public string Reason { get; set; }


        public TypeResult(TypeDefinition type, bool isPassing)
        {
            Type = type;
            IsPassing = isPassing;
        }


        TypeDefinition IFailingType.MonoTypeDefinition { get => Type; }
    }
}
