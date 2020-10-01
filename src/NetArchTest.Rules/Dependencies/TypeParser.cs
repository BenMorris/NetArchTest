namespace NetArchTest.Rules.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;   

    internal static class TypeParser
    {
        static readonly Type mono_TypeParserType = Type.GetType("Mono.Cecil.TypeParser, " + typeof(Mono.Cecil.TypeReference).Assembly);
        static readonly Type mono_TypeType = Type.GetType("Mono.Cecil.TypeParser+Type, " + typeof(Mono.Cecil.TypeReference).Assembly);
        static readonly MethodInfo mono_ParseTypeMethod = mono_TypeParserType.GetMethod("ParseType", BindingFlags.Instance | BindingFlags.NonPublic);
        static readonly FieldInfo mono_type_fullnameField = mono_TypeType.GetField("type_fullname", BindingFlags.Instance | BindingFlags.Public);
        static readonly FieldInfo mono_nested_namesField = mono_TypeType.GetField("nested_names", BindingFlags.Instance | BindingFlags.Public);
        static readonly FieldInfo mono_generic_argumentsField = mono_TypeType.GetField("generic_arguments", BindingFlags.Instance | BindingFlags.Public);
        static readonly FieldInfo mono_specsField = mono_TypeType.GetField("specs", BindingFlags.Instance | BindingFlags.Public);


        public static IEnumerable<string> Parse(string fullName, bool parseNames)
        {
            if (parseNames == false)
            {
                yield return fullName;
                yield break;
            }
           
            var monoTypeParser = Activator.CreateInstance(mono_TypeParserType, BindingFlags.Instance | BindingFlags.NonPublic, null, args: new object[] { fullName }, null);
            var monoType = mono_ParseTypeMethod.Invoke(monoTypeParser, new object[] { false });
            foreach(var token in WalkThroughMonoType(monoType))
            {
                yield return token;
            }
        }

        private static IEnumerable<string> WalkThroughMonoType(object monoType)
        {
            yield return mono_type_fullnameField.GetValue(monoType) as string;
            
            var nested = mono_nested_namesField.GetValue(monoType) as string[];
            if (nested != null)
            {
                foreach (var nestedName in nested)
                {
                    yield return nestedName;
                }
            } 

            var generics = mono_generic_argumentsField.GetValue(monoType) as object[];
            if (generics != null)
            {
                yield return "<";
                foreach (var generic in generics)
                {
                    foreach (var token in WalkThroughMonoType(generic))
                    {
                        yield return token;
                    }
                    yield return ",";
                }
            }

            var specs = mono_specsField.GetValue(monoType) as int[];
            if (specs != null)
            {
                for (int i = 0; i < specs.Length; ++i)
                {
                    if (specs[i] == -1)
                    {
                        yield return "*";
                    }
                    if (specs[i] == -2)
                    {
                        yield return "&";
                    }
                    if (specs[i] == -3)
                    {
                        yield return "[]";
                    }
                    if (specs[i] >= 2)
                    {
                        yield return "[,]";
                    }
                }
            }
        }
    }
}