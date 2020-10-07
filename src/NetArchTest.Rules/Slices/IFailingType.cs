namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Mono.Cecil;

    public interface IFailingType
    {
        TypeDefinition MonoTypeDefinition { get;  }
    }
}
