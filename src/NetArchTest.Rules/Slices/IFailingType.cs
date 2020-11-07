namespace NetArchTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Mono.Cecil;

    /// <summary>
    /// Type that failed the test.
    /// </summary>
    public interface IFailingType
    {
        /// <summary>
        /// System.Type that failed the test.
        /// </summary>
        /// <remarks>
        /// This property may be null if the test project does not have a direct dependency on the type.
        /// </remarks>
        Type Type { get;  }


        /// <summary>
        /// Name of the type that failed the test.
        /// </summary>       
        string TypeName { get; }
    }
}
