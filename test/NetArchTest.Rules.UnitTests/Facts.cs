using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Mono.Cecil;
using Xunit;
using Xunit.Abstractions;

namespace NetArchTest.Rules.UnitTests
{
    public class Facts
    {
        private readonly ITestOutputHelper _output;

        public Facts(ITestOutputHelper output)
        {
            _output = output;
        }

        void blabla()
        {
//            Bla bla;
//            
            List<Bla> blas;
            TimeSpan ts = TimeSpan.Zero;
            TimeSpan ts1 = TimeSpan.FromTicks(1);
            var days = new TimeSpan().Days;

            var type = new Bla().GetType();
//            List<string> strings = new List<string>();
//            
//            var type = typeof(Bla);
//            
//            blas = new List<Bla>();
//            
//            var bla1 = new Bla();
//
//            var x = new List<Bla>();
//
//            blas = new List<Bla>();
//            int y = blas.Count;
//            int i = 3;
//            i++;
            blas = new List<Bla>();
            _output.WriteLine(blas.ToString());
        }

        [Fact]
        void test()
        {
            var assemblyDef = Mono.Cecil.AssemblyDefinition
                .ReadAssembly(typeof(Facts).Module.FullyQualifiedName);

            var types = assemblyDef.Modules.SelectMany(m => m.GetTypes());
            var definition = types.First(t => t.Name == "Facts");
            var methodDefinition = definition.Methods.First(m => m.Name == "blabla");
            
            foreach (var bodyInstruction in methodDefinition.Body.Instructions)
            {
                _output.WriteLine(bodyInstruction.ToString());
                if (bodyInstruction.Operand != null)
                {
//                    _output.WriteLine(bodyInstruction.ToString());
                    _output.WriteLine("--" + bodyInstruction.Operand.GetType().ToString());
                    if (bodyInstruction.Operand is FieldReference fr)
                    {
                        _output.WriteLine("--" + fr.DeclaringType);
                    }
                    if (bodyInstruction.Operand is MethodReference mr)
                    {
                        _output.WriteLine("--" + mr.DeclaringType + "--" + mr.DeclaringType.IsGenericInstance);
                    }
                }
            }
                
        }
    }
    public class Bla{}
}