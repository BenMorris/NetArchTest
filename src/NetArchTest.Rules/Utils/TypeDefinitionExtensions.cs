using Mono.Cecil;

namespace NetArchTest.Rules.Utils
{
    internal static class TypeDefinitionExtensions 
    {
        public static string GetNamespace(this TypeDefinition type)
        {   
            if (type.IsNested && string.IsNullOrEmpty(type.Namespace))
            {
                if (type.DeclaringType != null)
                {
                    return type.DeclaringType.GetNamespace();    
                }
            }
            return type.Namespace;
        }
    }
}