namespace NetArchTest.Rules.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using NetArchTest.TestStructure.NameMatching.Namespace1;
    using NetArchTest.TestStructure.NameMatching.Namespace2;
    using NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3;
    using Xunit;

    public class TypesTests
    {
        [Fact(DisplayName = "System types should be excluded from the current domain.")]
        public void InCurrentDomain_SystemTypesExcluded()
        {
            var result = Types.InCurrentDomain().GetTypeDefinitions();

            Assert.DoesNotContain(result, t => t.FullName.StartsWith("System"));
            Assert.DoesNotContain(result, t => t.FullName.StartsWith("Microsoft"));
            Assert.DoesNotContain(result, t => t.FullName.StartsWith("netstandard"));
        }

        [Fact(DisplayName = "NetArchTest types should be excluded from the current domain.")]
        public void InCurrentDomain_NetArchTestTypesExcluded()
        {
            var result = Types.InCurrentDomain().GetTypeDefinitions();

            Assert.DoesNotContain(result, t => t.FullName.StartsWith("NetArchTest.Rules"));
            Assert.DoesNotContain(result, t => t.FullName.StartsWith("Mono.Cecil"));
        }

        [Fact(DisplayName = "Nested public types should be included in the current domain.")]
        public void InCurrentDomain_NestedPublicTypesPresent_Returned()
        {
            var result = Types.InCurrentDomain().GetTypeDefinitions();
            Assert.Contains(result, t => t.FullName.StartsWith("NetArchTest.TestStructure.Nested.NestedPublic/NestedPublicClass"));
        }

        [Fact(DisplayName = "Nested private types should be included in the current domain.")]
        public void InCurrentDomain_NestedPrivateTypesPresent_Returned()
        {
            var result = Types.InCurrentDomain().GetTypeDefinitions();
            Assert.Contains(result, t => t.FullName.StartsWith("NetArchTest.TestStructure.Nested.NestedPrivate/NestedPrivateClass"));
        }

        [Fact(DisplayName = "A types collection can be created from a namespace.")]
        public void InNamespace_TypesReturned()
        {
            var result = Types.InNamespace("NetArchTest.TestStructure.NameMatching").GetTypes();

            Assert.Equal(5, result.Count()); // Five types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassA3), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
        }

        [Fact(DisplayName = "A types collection can be created from a filename.")]
        public void FromFile_TypesReturned()
        {
            // Arrange
            var expected = Types.InCurrentDomain().That().ResideInNamespace("NetArchTest.TestStructure").GetTypeDefinitions().Count();

            // Act
            var result = Types.FromFile("NetArchTest.TestStructure.dll").GetTypes();

            // Assert
            Assert.Equal(expected, result.Count());
            Assert.All(result, r => r.FullName.StartsWith("NetArchTest.TestStructure"));
        }

        [Fact(DisplayName = "A types collection can be created from a path.")]
        public void FromPath_TypesReturned()
        {
            // Arrange
            var expected = Types.InCurrentDomain().That().ResideInNamespace("NetArchTest.TestStructure").GetTypeDefinitions().Count();
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Act
            var result = Types.FromPath(dir).That().ResideInNamespace("NetArchTest.TestStructure").GetTypes();

            // Assert
            Assert.Equal(expected, result.Count());
        }

        [Fact(DisplayName = "When loading a type a BadImageFormatException will be caught and an empty list will be returned.")]
        public void FromFile_BadImage_CaughtAndEmptyListReturned()
        {
            // Act
            var result = Types.FromFile("NetArchTest.TestStructure.pdb").GetTypes();

            // Assert
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Any compiler generated classes will be ignored in a types list.")]
        public void InNamespace_CompilerGeneratedClasses_NotReturned()
        {
            // Act
            var result = Types.InNamespace("NetArchTest.TestStructure.Dependencies.Search").GetTypes();

            // Assert
            var generated = result.Any(r => r.CustomAttributes.Any(x => x?.AttributeType?.FullName == typeof(CompilerGeneratedAttribute).FullName));
            Assert.False(generated);
        }
    }
}
