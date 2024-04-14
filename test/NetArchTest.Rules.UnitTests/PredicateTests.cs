using NetArchTest.TestStructure.NameMatching.Namespace3.A;
using NetArchTest.TestStructure.NameMatching.Namespace3.B;

namespace NetArchTest.Rules.UnitTests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using NetArchTest.CrossAssemblyTest.A;
    using NetArchTest.CrossAssemblyTest.B;
    using NetArchTest.TestStructure.Abstract;
    using NetArchTest.TestStructure.Classes;
    using NetArchTest.TestStructure.CustomAttributes;
    using NetArchTest.TestStructure.Dependencies;
    using NetArchTest.TestStructure.Dependencies.Implementation;
    using NetArchTest.TestStructure.Generic;
    using NetArchTest.TestStructure.Inheritance;
    using NetArchTest.TestStructure.Interfaces;
    using NetArchTest.TestStructure.NameMatching.Namespace1;
    using NetArchTest.TestStructure.NameMatching.Namespace2;
    using NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3;
    using NetArchTest.TestStructure.NameMatching.Namespace3;
    using NetArchTest.TestStructure.NamespaceMatching.Namespace1;
    using NetArchTest.TestStructure.NamespaceMatching.NamespaceA;
    using NetArchTest.TestStructure.Nested;
    using NetArchTest.TestStructure.Scope;
    using NetArchTest.TestStructure.Sealed;
    using NetArchTest.TestStructure.Mutability;
    using Xunit;
    using NetArchTest.TestStructure.Nullable;
    using NetArchTest.TestStructure.Dependencies.Examples;
    using NetArchTest.TestStructure.Enumerations;

    public class PredicateTests
    {
        [Fact(DisplayName = "Types can be selected by name name.")]
        public void HaveName_MatchFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .HaveName("ClassA1").GetTypes();

            Assert.Single(result); // Only one type found
            Assert.Equal<Type>(typeof(ClassA1), result.First()); // The correct type found
        }

        [Fact(DisplayName = "Types can be selected if they do not have a specific name.")]
        public void DoNotHaveName_MatchFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotHaveName("ClassA1").GetTypes();

            Assert.Equal(8, result.Count()); // Eight types found
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
            Assert.Contains<Type>(typeof(ClassA3), result);
            Assert.Contains<Type>(typeof(SomeThing), result);
            Assert.Contains<Type>(typeof(SomethingElse), result);
            Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types can be selected by the start of their name.")]
        public void HaveNameStarting_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .HaveNameStartingWith("SomeT").GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Contains<Type>(typeof(SomeThing), result);
            Assert.Contains<Type>(typeof(SomethingElse), result);
        }

        [Fact(DisplayName = "Types can be selected by the start of their name using a StringComparison.")]
        public void HaveNameStarting_UsingExplicitStringComparison_MatchesFound_ClassesSelected()
        {
	        var result = Types
		        .InAssembly(Assembly.GetAssembly(typeof(SomeThing)))
		        .That()
		        .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
		        .And()
		        .HaveNameStartingWith("SomeT", StringComparison.Ordinal).GetTypes();

	        Assert.Single(result); // One type found
	        Assert.Contains<Type>(typeof(SomeThing), result);
	        Assert.DoesNotContain<Type>(typeof(SomethingElse), result);
        }

        [Fact(DisplayName = "Types can be selected if their name does not have a specific start.")]
        public void DoNotHaveNameStarting_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotHaveNameStartingWith("ClassA").GetTypes();

            Assert.Equal(6, result.Count()); // Six types found
            Assert.Contains<Type>(typeof(ClassB1), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
            Assert.Contains<Type>(typeof(SomeThing), result);
            Assert.Contains<Type>(typeof(SomethingElse), result);
            Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types can be selected if their name does not have a specific start using a StringComparison.")]
        public void DoNotHaveNameStarting_UsingExplicitStringComparison_MatchesFound_ClassesSelected()
        {
	        var result = Types
		        .InAssembly(Assembly.GetAssembly(typeof(SomeThing)))
		        .That()
		        .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace3")
		        .And()
		        .DoNotHaveNameStartingWith("SomeT", StringComparison.Ordinal).GetTypes();

	        Assert.Equal(3, result.Count()); // Three types found
	        Assert.DoesNotContain<Type>(typeof(SomeThing), result);
	        Assert.Contains<Type>(typeof(SomethingElse), result);
	        Assert.Contains<Type>(typeof(SomeEntity), result);
	        Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types can be selected by the end of their name.")]
        public void HaveNameEnding_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .HaveNameEndingWith("Entity").GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types can be selected by the end of their name using a StringComparison.")]
        public void HaveNameEnding_UsingExplicitStringComparison_MatchesFound_ClassesSelected()
        {
	        var result = Types
		        .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
		        .That()
		        .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
		        .And()
		        .HaveNameEndingWith("Entity", StringComparison.Ordinal).GetTypes();

	        Assert.Single(result); // One type found
	        Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.DoesNotContain<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types can be selected if their name does not have a specific end.")]
        public void DoNotHaveNameEnding_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotHaveNameEndingWith("A1").GetTypes();

            Assert.Equal(8, result.Count()); // three types found
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassA3), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
            Assert.Contains<Type>(typeof(SomeThing), result);
            Assert.Contains<Type>(typeof(SomethingElse), result);
            Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types can be selected by a regular expression.")]
        public void HaveNameMatching_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .HaveNameMatching(@"Class\w1").GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
        }

        [Fact(DisplayName = "Types can be selected if they do not conform to a regular expression.")]
        public void DoNotHaveNameMatching_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotHaveNameMatching(@"Class\w1").GetTypes();

            Assert.Equal(7, result.Count()); // Three types found
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassA3), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
            Assert.Contains<Type>(typeof(SomeThing), result);
            Assert.Contains<Type>(typeof(SomethingElse), result);
            Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types can be selected by a the presence of a custom attribute.")]
        public void HaveCustomAttribute_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.CustomAttributes")
                .And()
                .HaveCustomAttribute(typeof(ClassCustomAttribute)).GetTypes();

            Assert.Single(result); // One type found
            Assert.Contains<Type>(typeof(AttributePresent), result);
        }

        [Fact(DisplayName = "Types can be selected by the absence of a custom attribute.")]
        public void DoNotHaveCustomAttribute_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.CustomAttributes")
                .And()
                .DoNotHaveCustomAttribute(typeof(ClassCustomAttribute)).GetTypes();

            Assert.Equal(4, result.Count()); // Four types found
            Assert.Contains<Type>(typeof(NoAttributes), result);
            Assert.Contains<Type>(typeof(ClassCustomAttribute), result);
            Assert.Contains<Type>(typeof(InheritAttributePresent), result);
            Assert.Contains<Type>(typeof(InheritClassCustomAttribute), result);
        }

        [Fact(DisplayName = "Types can be selected by the presence of an inherited custom attribute.")]
        public void HaveInheritCustomAttribute_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.CustomAttributes")
                .And()
                .HaveCustomAttributeOrInherit(typeof(ClassCustomAttribute)).GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Contains<Type>(typeof(AttributePresent), result);
            Assert.Contains<Type>(typeof(InheritAttributePresent), result);
        }

        [Fact(DisplayName = "Types can be selected by the absence of an inherited custom attribute.")]
        public void DoNotHaveInheritCustomAttribute_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.CustomAttributes")
                .And()
                .DoNotHaveCustomAttributeOrInherit(typeof(ClassCustomAttribute)).GetTypes();

            Assert.Equal(3, result.Count()); // Three types found
            Assert.Contains<Type>(typeof(NoAttributes), result);
            Assert.Contains<Type>(typeof(ClassCustomAttribute), result);          
            Assert.Contains<Type>(typeof(InheritClassCustomAttribute), result);
        }

        [Fact(DisplayName = "Types can be selected if they inherit from a type.")]
        public void Inherit_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Inheritance")
                .And()
                .Inherit(typeof(BaseClass)).GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Contains<Type>(typeof(DerivedClass), result);
            Assert.Contains<Type>(typeof(DerivedDerivedClass), result);
        }

        [Fact(DisplayName = "Types can be selected if they inherit from a type from a different assembly")]
        public void Inherit_MatchesFound_ClassesSelected_AcrossAssemblies()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(DerivedClassFromB)))
                .That()
                .Inherit(typeof(BaseClassFromA))
                .GetTypes();

            Assert.Equal(2, result.Count());
            Assert.Contains(typeof(DerivedClassFromB), result);
            Assert.Contains(typeof(AnotherDerivedClassFromB), result);
        }

        [Fact(DisplayName = "Types can be selected if they do not inherit from a type.")]
        public void DoNotInherit_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Inheritance")
                .And()
                .DoNotInherit(typeof(BaseClass)).GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Contains<Type>(typeof(BaseClass), result);
            Assert.Contains<Type>(typeof(NotDerivedClass), result);
        }

        [Fact(DisplayName = "Types can be selected if they implement an interface.")]
        public void ImplementInterface_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Interfaces")
                .And()
                .ImplementInterface(typeof(IExample)).GetTypes();

            Assert.Single(result); // One type found
            Assert.Contains<Type>(typeof(ImplementsExampleInterface), result);
        }

        [Fact(DisplayName = "Types can be selected if they do not implement an interface.")]
        public void DoNotImplementInterface_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Interfaces")
                .And()
                .DoNotImplementInterface(typeof(IExample)).GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Contains<Type>(typeof(IExample), result);
            Assert.Contains<Type>(typeof(DoesNotImplementInterface), result);
        }

        [Fact(DisplayName = "Types can be selected if they are abstract.")]
        public void AreAbstract_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Abstract")
                .And()
                .AreAbstract().GetTypes();

            Assert.Single(result); // One type found
            Assert.Contains<Type>(typeof(AbstractClass), result);
        }

        [Fact(DisplayName = "Types can be selected if they are not abstract.")]
        public void AreNotAbstract_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Abstract")
                .And()
                .AreNotAbstract().GetTypes();

            Assert.Single(result); // One type found
            Assert.Contains<Type>(typeof(ConcreteClass), result);
        }

        [Fact(DisplayName = "Types can be selected if they are classes.")]
        public void AreClasses_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Classes")
                .And()
                .AreClasses().GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Contains<Type>(typeof(ExampleClass), result);
            Assert.Contains<Type>(typeof(ExampleStaticClass), result);
        }

        [Fact(DisplayName = "Types can be selected if they are not classes.")]
        public void AreNotClasses_MatchesFound_ClassesSelected ()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Classes")
                .And()
                .AreNotClasses().GetTypes();

            Assert.Single(result); // One type found
            Assert.Contains<Type>(typeof(IExampleInterface), result);
        }

        [Fact(DisplayName = "Types can be selected if they have generic parameters.")]
        public void AreGeneric_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Generic")
                .And()
                .AreGeneric().GetTypes();

            Assert.Single(result); // One type found
            Assert.Contains<Type>(typeof(GenericType<>), result);
        }

        [Fact(DisplayName = "Types can be selected if they do not have generic parameters.")]
        public void AreNotGeneric_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Generic")
                .And()
                .AreNotGeneric().GetTypes();

            Assert.Single(result); // One type found
            Assert.Contains<Type>(typeof(NonGenericType), result);
        }

        [Fact(DisplayName = "Types can be selected if they are interfaces.")]
        public void AreInterfaces_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Classes")
                .And()
                .AreInterfaces().GetTypes();

            Assert.Single(result); // One type found
            Assert.Contains<Type>(typeof(IExampleInterface), result);
        }

        [Fact(DisplayName = "Types can be selected if they are not interfaces.")]
        public void AreNotInterfaces_MatchesFound_ClassesSelected ()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Classes")
                .And()
                .AreNotInterfaces().GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Contains<Type>(typeof(ExampleClass), result);
            Assert.Contains<Type>(typeof(ExampleStaticClass), result);
        }

        [Fact(DisplayName = "Types can be selected if they are static.")]
        public void AreStatic_MatchesFound_ClassesSelected()
        {
	        var result = Types
		        .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
		        .That()
		        .ResideInNamespace("NetArchTest.TestStructure.Classes")
		        .And()
		        .AreStatic().GetTypes();

	        Assert.Single(result); // One type found
	        Assert.Contains<Type>(typeof(ExampleStaticClass), result);
        }

        [Fact(DisplayName = "Types can be selected if they are not static.")]
        public void AreNotStatic_MatchesFound_ClassesSelected()
        {
	        var result = Types
		        .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
		        .That()
		        .ResideInNamespace("NetArchTest.TestStructure.Classes")
		        .And()
		        .AreNotStatic().GetTypes();

	        Assert.Equal(2, result.Count()); // Two types found
            Assert.Contains<Type>(typeof(ExampleClass), result);
	        Assert.Contains<Type>(typeof(IExampleInterface), result);
        }

        [Fact(DisplayName = "Types can be selected if they are nested.")]
        public void AreNested_MatchesFound_ClassesSelected()
        {
                var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nested")
                .And()
                .AreNested().GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Equal("NestedPrivateClass", result.First().Name);
            Assert.Equal("NestedPublicClass", result.Last().Name);
        }

        [Fact(DisplayName = "Types can be selected if they are nested and public.")]
        public void AreNestedPublic_MatchesFound_ClassesSelected()
        {
            var result = Types
            .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
            .That()
            .ResideInNamespace("NetArchTest.TestStructure.Nested")
            .And()
            .AreNestedPublic().GetTypes();

            Assert.Single(result); // One types found
            Assert.Equal("NestedPublicClass", result.First().Name);
        }

        [Fact(DisplayName = "Types can be selected if they are nested and private.")]
        public void AreNestedPrivate_MatchesFound_ClassesSelected()
        {
            var result = Types
            .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
            .That()
            .ResideInNamespace("NetArchTest.TestStructure.Nested")
            .And()
            .AreNestedPrivate().GetTypes();

            Assert.Single(result); // One types found
            Assert.Equal("NestedPrivateClass", result.First().Name);
        }

        [Fact(DisplayName = "Types can be selected if they are not nested.")]
        public void AreNotNested_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nested")
                .And()
                .AreNotNested().GetTypes();

            Assert.Equal(3, result.Count()); // Three types found
            Assert.Equal("NestedPrivate", result.First().Name);
            Assert.Equal("NestedPublic", result.Skip(1).First().Name);
            Assert.Equal("NotNested", result.Last().Name);
        }

        [Fact(DisplayName = "Types can be selected if they are not nested and public.")]
        public void AreNotNestedPublic_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nested")
                .And()
                .AreNotNestedPublic().GetTypes();

            Assert.Equal(4, result.Count()); // Four types found
            var match = result.Any(r => r.Name == "NestedPublicClass");
            Assert.False(match);
        }

        [Fact(DisplayName = "Types can be selected if they are not nested and private.")]
        public void AreNotNestedPrivate_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nested")
                .And()
                .AreNotNestedPrivate().GetTypes();

            Assert.Equal(4, result.Count()); // Four types found
            var match = result.Any(r => r.Name == "NestedPrivateClass");
            Assert.False(match);
        }

        [Fact(DisplayName = "Types can be selected for being declared as public.")]
        public void ArePublic_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Scope")
                .And()
                .ArePublic().GetTypes();

            Assert.Equal(2, result.Count()); 
            Assert.Contains<Type>(typeof(PublicClass), result);
            Assert.Contains<Type>(typeof(PublicClass.PublicClassInternal), result);
        }

        [Fact(DisplayName = "Types can be selected for not being declared as public.")]
        public void AreNotPublic_MatchesFound_ClassSelected ()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Scope")
                .And()
                .AreNotPublic().GetTypes();

            Assert.Equal(2, result.Count());
            Assert.Contains<Type>(typeof(InternalClass), result);
            Assert.Contains<Type>(typeof(InternalClass.InternalClassNested), result);
        }

        [Fact(DisplayName = "Types can be selected for being declared as sealed.")]
        public void AreSealed_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Sealed")
                .And()
                .AreSealed().GetTypes();

            Assert.Single(result); // One result
            Assert.Contains<Type>(typeof(SealedClass), result);
        }

        [Fact(DisplayName = "Types can be selected for not being declared as sealed.")]
        public void AreNotSealed_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Sealed")
                .And()
                .AreNotSealed().GetTypes();

            Assert.Single(result); // One result
            Assert.Contains<Type>(typeof(NotSealedClass), result);
        }

        [Fact(DisplayName = "Types can be selected for being immutable.")]
        public void AreImmutable_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Mutability")
                .And()
                .AreImmutable().GetTypes();

            Assert.Equal(3, result.Count()); // Three types found
            Assert.Contains<Type>(typeof(ImmutableClass1), result);
            Assert.Contains<Type>(typeof(ImmutableClass2), result);
            Assert.Contains<Type>(typeof(ImmutableClass3), result);
        }

        [Fact(DisplayName = "Types can be selected for being mutable.")]
        public void AreMutable_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Mutability")
                .And()
                .AreMutable().GetTypes();

            Assert.Equal(3, result.Count()); // Three types found
            Assert.Contains<Type>(typeof(PartiallyMutableClass1), result);
            Assert.Contains<Type>(typeof(PartiallyMutableClass2), result);
            Assert.Contains<Type>(typeof(MutableClass), result);
        }

        [Fact(DisplayName = "Types can be selected for having only nullable memebers.")]
        public void AreNullable_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nullable")
                .And()
                .AreNotNested() // ignore nested helper types
                .And()
                .AreClasses()
                .And()
                .OnlyHaveNullableMembers().GetTypes();

            Assert.Single(result); // One result
            Assert.Contains<Type>(typeof(NullableClass), result);
        }

        [Fact(DisplayName = "Types can be selected for having non-nullable memebers.")]
        public void AreNonNullable_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nullable")
                .And()
                .AreNotNested() // ignore nested helper types
                .And()
                .ArePublic()
                .And()
                .HaveSomeNonNullableMembers().GetTypes();

            Assert.Equal(4, result.Count()); // Four types found
            Assert.Contains<Type>(typeof(NonNullableClass1), result);
            Assert.Contains<Type>(typeof(NonNullableClass2), result);
            Assert.Contains<Type>(typeof(NonNullableClass3), result);
            Assert.Contains<Type>(typeof(NonNullableClass4), result);
        }

        [Fact(DisplayName = "Types can be selected if they reside in a namespace.")]
        public void ResideInNamespace_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace1")
                .GetTypes();

            Assert.Equal(3, result.Count()); // Three types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
        }

        [Fact(DisplayName = "Types can be selected if they do not reside in a namespace.")]
        public void DoNotResideInNamespace_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2")
                .GetTypes();

            Assert.Equal(7, result.Count()); // Seven types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
            Assert.Contains<Type>(typeof(SomeThing), result);
            Assert.Contains<Type>(typeof(SomethingElse), result);
            Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types can be selected if they reside in a namespace that matches a regular expression.")]
        public void ResideInNamespaceMatching_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespaceMatching(@"NetArchTest.TestStructure.NamespaceMatching.Namespace\d")
                .GetTypes();

            Assert.Single(result); // One type found
            Assert.Contains<Type>(typeof(Match1), result);
        }

        [Fact(DisplayName = "Types can be selected if they do not reside in a namespace that matches a regular expression.")]
        public void DoNotResideInNamespaceMatching_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NamespaceMatching")
                .And()
                .DoNotResideInNamespaceMatching(@"NetArchTest.TestStructure.NamespaceMatching.Namespace\d")
                .GetTypes();

            Assert.Single(result); // One type found
            Assert.Contains<Type>(typeof(MatchA), result);
        }

        [Fact(DisplayName = "Types can be selected if they reside in a namespace that starts with a name part.")]
        public void ResideInNamespaceStartingWith_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespaceStartingWith("NetArchTest.TestStructure.NameMatching")
                .GetTypes();

            Assert.Equal(9, result.Count()); // Nine types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassA3), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(SomeThing), result);
            Assert.Contains<Type>(typeof(SomethingElse), result);
            Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types can be selected if they do not reside in a namespace that start with name part.")]
        public void DoNotResideInNamespaceStartingWith_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespaceStartingWith("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotResideInNamespaceStartingWith("NetArchTest.TestStructure.NameMatching.Namespace2")
                .GetTypes();

            Assert.Equal(7, result.Count()); // Seven types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
            Assert.Contains<Type>(typeof(SomeThing), result);
            Assert.Contains<Type>(typeof(SomethingElse), result);
            Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types can be selected if they reside in a namespace that ends with a name part.")]
        public void ResideInNamespaceEndingWith_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespaceEndingWith(".NameMatching.Namespace1")
                .GetTypes();

            Assert.Equal(3, result.Count()); // Three types found
            Assert.Contains<Type>(typeof(ClassA1), result);
        }

        [Fact(DisplayName = "Types can be selected if they do not reside in a namespace that end with name part.")]
        public void DoNotResideInNamespaceEndingWith_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespaceStartingWith("NetArchTest.TestStructure.NameMatching")
                .GetTypes();

            Assert.Equal(9, result.Count()); // Nine types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassA3), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
            Assert.Contains<Type>(typeof(SomeThing), result);
            Assert.Contains<Type>(typeof(SomethingElse), result);
            Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types can be selected if they reside in a namespace that contains a name part.")]
        public void ResideInNamespaceContaining_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespaceContaining(".NameMatching.")
                .GetTypes();

            Assert.Equal(9, result.Count()); // Nine types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassA3), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
            Assert.Contains<Type>(typeof(SomeThing), result);
            Assert.Contains<Type>(typeof(SomethingElse), result);
            Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types (nested) can be selected if they reside in a namespace that contains a name part.")]
        public void ResideInNamespaceContaining_NestedClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespaceContaining("Nested")
                .GetTypes();

            Assert.Equal(19, result.Count()); 
            Assert.Contains<Type>(typeof(NestedPublic.NestedPublicClass), result);
        }

        [Fact(DisplayName = "Types can be selected if they do not reside in a namespace that contains name part.")]
        public void DoNotResideInNamespaceContaining_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespaceStartingWith("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotResideInNamespaceContaining("Namespace2")
                .GetTypes();

            Assert.Equal(7, result.Count()); // Three types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
            Assert.Contains<Type>(typeof(SomeThing), result);
            Assert.Contains<Type>(typeof(SomethingElse), result);
            Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Selecting by namespace will return types in nested namespaces.")]
        public void ResideInNamespace_Nested_AllClassReturned()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .GetTypes();

            // Should return all the types that are in three nested namespaces
            Assert.Equal(9, result.Count()); // Nine types found
            Assert.Contains<Type>(typeof(ClassA1), result);
            Assert.Contains<Type>(typeof(ClassA2), result);
            Assert.Contains<Type>(typeof(ClassA3), result);
            Assert.Contains<Type>(typeof(ClassB1), result);
            Assert.Contains<Type>(typeof(ClassB2), result);
            Assert.Contains<Type>(typeof(SomeThing), result);
            Assert.Contains<Type>(typeof(SomethingElse), result);
            Assert.Contains<Type>(typeof(SomeEntity), result);
            Assert.Contains<Type>(typeof(SomeIdentity), result);
        }

        [Fact(DisplayName = "Types can be selected if they have a dependency on a specific item.")]
        public void HaveDepencency_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .HaveDependencyOn(typeof(ExampleDependency).FullName)
                .GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Equal<Type>(typeof(HasDependencies), result.First());
            Assert.Equal<Type>(typeof(HasDependency), result.Last());
        }

        [Fact(DisplayName = "Types can be selected if they have a dependency on any item in a list.")]
        public void HaveDepencencyOnAny_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .HaveDependencyOnAny(new[] { typeof(ExampleDependency).FullName, typeof(AnotherExampleDependency).FullName })
                .GetTypes();

            Assert.Equal(3, result.Count()); // Three types found - i.e. the classes with dependencies on at least one of the items
            Assert.Equal<Type>(typeof(HasAnotherDependency), result.First());
            Assert.Equal<Type>(typeof(HasDependencies), result.Skip(1).First());
            Assert.Equal<Type>(typeof(HasDependency), result.Last());
        }

        [Fact(DisplayName = "Types can be selected if they have a dependency on all the items in a list.")]
        public void HaveDepencencyOnAll_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .HaveDependencyOnAll(new[] { typeof(ExampleDependency).FullName, typeof(AnotherExampleDependency).FullName })
                .GetTypes();

            Assert.Single(result); // Only one type found - i.e. the class with dependencies on both items
            Assert.Equal<Type>(typeof(HasDependencies), result.First()); // The correct type found
        }

        [Fact(DisplayName = "Types can be selected if they only have a dependency on any item in a list.")]
        public void OnlyHaveDependenciesOn_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .OnlyHaveDependenciesOn(new[] { typeof(ExampleDependency).FullName, "System" })
                .GetTypes();

            Assert.Equal(2, result.Count());          
            Assert.Equal<Type>(typeof(HasDependency), result.First());
            Assert.Equal<Type>(typeof(NoDependency), result.Skip(1).First());
        }

        [Fact(DisplayName = "Types can be selected if they do not have a dependency on another type.")]
        public void DoNotHaveDepencency_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .DoNotHaveDependencyOn(typeof(ExampleDependency).FullName)
                .GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Equal<Type>(typeof(HasAnotherDependency), result.First());
            Assert.Equal<Type>(typeof(NoDependency), result.Last());
        }

        [Fact(DisplayName = "Types can be selected if they do not have a dependency on any item in a list.")]
        public void DoNotHaveDependencyOnAny_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .DoNotHaveDependencyOnAny(new[] { typeof(ExampleDependency).FullName, typeof(AnotherExampleDependency).FullName })
                .GetTypes();

            Assert.Single(result); // Only one type found
            Assert.Equal<Type>(typeof(NoDependency), result.First()); // The correct type found - i.e. it's the only type with no matching dependencies at all
        }

        [Fact(DisplayName = "Types can be selected if they do not have a dependency on all the items in a list.")]
        public void DoNotHaveDependencyOnAll_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .DoNotHaveDependencyOnAll(new[] { typeof(ExampleDependency).FullName, typeof(AnotherExampleDependency).FullName })
                .GetTypes();

            Assert.Equal(3, result.Count()); // Three types found - i.e. the classes with dependencies on at least one of the items
            Assert.Equal<Type>(typeof(HasAnotherDependency), result.First());
            Assert.Equal<Type>(typeof(HasDependency), result.Skip(1).First());
            Assert.Equal<Type>(typeof(NoDependency), result.Last());
        }

        [Fact(DisplayName = "Types can be selected if they do not have a dependency on any item in a list.")]
        public void HaveDependenciesOtherThan_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .HaveDependenciesOtherThan(new[] { typeof(ExampleDependency).FullName, "System" })
                .GetTypes();

            Assert.Equal(2, result.Count());
            Assert.Equal<Type>(typeof(HasAnotherDependency), result.First());
            Assert.Equal<Type>(typeof(HasDependencies), result.Skip(1).First());
        }

        [Fact(DisplayName = "Types can be selected according to a custom rule.")]
        public void MeetCustomRule_MatchesFound_ClassSelected()
        {
            // Create a custom rule that selects "ClassA1"
            var rule = new CustomRuleExample(t => t.Name.Equals("ClassA1", StringComparison.InvariantCultureIgnoreCase));

            // Use the custom rule
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .MeetCustomRule(rule)
                .GetTypes();

            // ClassA1 has been returned
            Assert.Single(result);
            Assert.Equal<Type>(typeof(ClassA1), result.First());

            // The custom rule was executed at least once
            Assert.True(rule.TestMethodCalled);
        }
        
        [Fact(DisplayName = "Custom rule that returns false doesnt evaluate twice.")]
        public void GetResult_Doesnt_Evaluate_Twice()
        {
            var customRule = new CustomRuleExample(_ => false);

            var t = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .HaveName("ClassA1")
                .Should()
                .MeetCustomRule(customRule)
                .GetResult();
            
            Assert.True(customRule.TimesTimesCalled == 1);
        }

        [Fact(DisplayName = "Types can be selected if they are enumerations.")]
        public void AreEnums_MatchesFound_EnumsSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Enumerations")
                .And()
                .AreEnums().GetTypes();

            Assert.Equal(2, result.Count()); // Two types found
            Assert.Contains<Type>(typeof(ExampleEnum), result);
            Assert.Contains<Type>(typeof(FlagEnum), result);
        }

        [Fact(DisplayName = "Types can be selected if they are not enumerations.")]
        public void AreNotEnums_MatchesFound_EnumsSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Enumerations")
                .And()
                .AreNotEnums().GetTypes();

            Assert.Single(result); // One type found
            Assert.Contains<Type>(typeof(NoEnum), result);
        }
    }
}
