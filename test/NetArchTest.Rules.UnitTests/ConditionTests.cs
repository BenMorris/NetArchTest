using NetArchTest.TestStructure.Classes;

namespace NetArchTest.Rules.UnitTests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using NetArchTest.TestStructure.CustomAttributes;
    using NetArchTest.TestStructure.Dependencies.Examples;
    using NetArchTest.TestStructure.Dependencies.Implementation;
    using NetArchTest.TestStructure.Inheritance;
    using NetArchTest.TestStructure.Interfaces;
    using NetArchTest.TestStructure.NameMatching.Namespace1;
    using NetArchTest.TestStructure.Nested;
    using Xunit;
    using static NetArchTest.TestStructure.Nested.NestedPublic;

    public class ConditionTests
    {
        [Fact(DisplayName = "Types can be selected by name.")]
        public void HaveName_MatchFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3")
                .Should()
                .HaveName("ClassB2").GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not have a specific name.")]
        public void NotHaveName_MatchFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace1")
                .Should()
                .NotHaveName("ClassB2").GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected by the start of their name.")]
        public void HaveNameStarting_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace3")
                .Should()
                .HaveNameStartingWith("Class").GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected by the start of their name using a StringComparison.")]
        public void HaveNameStarting_UsingStringComparison_MatchesFound_ClassesSelected()
        {
	        var result = Types
		        .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
		        .That()
		        .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace3")
		        .Should()
		        .HaveNameStartingWith("Some", StringComparison.Ordinal).GetResult();

	        Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if their name does not have a specific start.")]
        public void NotHaveNameStarting_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .Should()
                .NotHaveNameStartingWith("X").GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if their name does not have a specific start using a StringComparison.")]
        public void NotHaveNameStarting_UsingStringComparison_MatchesFound_ClassesSelected()
        {
	        var result = Types
		        .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
		        .That()
		        .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
		        .Should()
		        .NotHaveNameStartingWith("s", StringComparison.Ordinal).GetResult();

	        Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected by the end of their name.")]
        public void HaveNameEnding_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3")
                .Should()
                .HaveNameEndingWith("B2").GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected by the end of their name using a StringComparison.")]
        public void HaveNameEnding_UsingStringComparison_MatchesFound_ClassesSelected()
        {
	        var result = Types
		        .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
		        .That()
		        .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3.B")
		        .Should()
		        .HaveNameEndingWith("ntity").GetResult();

	        Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if their name does not have a specific end.")]
        public void NotHaveNameEnding_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2.Namespace1")
                .Should()
                .NotHaveNameEndingWith("B2").GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if their name does not have a specific end using a StringComparison.")]
        public void NotHaveNameEnding_UsingStringComparison_MatchesFound_ClassesSelected()
        {
	        var result = Types
		        .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
		        .That()
		        .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace3")
		        .Should()
		        .NotHaveNameEndingWith("ENTITY", StringComparison.Ordinal).GetResult();

	        Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected by a regular expression.")]
        public void HaveNameMatching_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .And()
                .DoNotResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace3")
                .Should()
                .HaveNameMatching(@"Class\w\d").GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not conform to a regular expression.")]
        public void NotHaveNameMatching_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .Should()
                .NotHaveNameMatching(@"X\w").GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected by a the presence of a custom attribute.")]
        public void HaveCustomAttribute_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.CustomAttributes")
                .And()
                .HaveName("AttributePresent")
                .Should()
                .HaveCustomAttribute(typeof(ClassCustomAttribute)).GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected by the absence of a custom attribute.")]
        public void NotHaveCustomAttribute_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.CustomAttributes")
                .And()
                .DoNotHaveName("AttributePresent")
                .Should()
                .NotHaveCustomAttribute(typeof(ClassCustomAttribute)).GetResult();

            Assert.True(result.IsSuccessful);
        }


        [Fact(DisplayName = "Types can be selected by a the presence of an inherited custom attribute.")]
        public void HaveInheritCustomAttribute_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.CustomAttributes")
                .And()
                .HaveName("InheritAttributePresent")
                .Should()
                .HaveCustomAttributeOrInherit(typeof(ClassCustomAttribute)).GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected by the absence of an inherited custom attribute.")]
        public void NotHaveInheritCustomAttribute_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.CustomAttributes")
                .And()
                .DoNotHaveNameEndingWith("AttributePresent")
                .Should()
                .NotHaveCustomAttributeOrInherit(typeof(ClassCustomAttribute)).GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they inherit from a type.")]
        public void Inherit_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Inheritance")
                .And()
                .HaveNameStartingWith("Derived")
                .Should()
                .Inherit(typeof(BaseClass)).GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not inherit from a type.")]
        public void NotInherit_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Inheritance")
                .And()
                .DoNotHaveNameStartingWith("Derived")
                .Should()
                .NotInherit(typeof(BaseClass)).GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they implement an interface.")]
        public void ImplementInterface_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Interfaces")
                .And()
                .HaveNameStartingWith("Implements")
                .Should()
                .ImplementInterface(typeof(IExample)).GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not implement an interface.")]
        public void NotImplementInterface_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Interfaces")
                .And()
                .DoNotHaveNameStartingWith("Implements")
                .Should()
                .NotImplementInterface(typeof(IExample)).GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are abstract.")]
        public void AreAbstract_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Abstract")
                .And()
                .HaveNameStartingWith("Abstract")
                .Should()
                .BeAbstract().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are not abstract.")]
        public void AreNotAbstract_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Abstract")
                .And()
                .DoNotHaveNameStartingWith("Abstract")
                .Should()
                .NotBeAbstract().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are classes.")]
        public void AreClasses_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Classes")
                .And()
                .HaveNameEndingWith("Class")
                .Should()
                .BeClasses().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are not classes.")]
        public void AreNotClasses_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Classes")
                .And()
                .HaveNameEndingWith("Interface")
                .Should()
                .NotBeClasses().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they have generic parameters.")]
        public void AreGeneric_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Generic")
                .And()
                .HaveNameStartingWith("Generic")
                .Should()
                .BeGeneric().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not have generic parameters.")]
        public void AreNotGeneric_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Generic")
                .And()
                .HaveNameStartingWith("NonGeneric")
                .Should()
                .NotBeGeneric().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are interfaces.")]
        public void AreInterfaces_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Classes")
                .And()
                .HaveNameEndingWith("Interface")
                .Should()
                .BeInterfaces().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are not interfaces.")]
        public void AreNotInterfaces_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Classes")
                .And()
                .HaveNameEndingWith("Class")
                .Should()
                .NotBeInterfaces().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are static.")]
        public void AreStatic_MatchesFound_ClassesSelected()
        {
	        var result = Types
		        .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
		        .That()
		        .ResideInNamespace("NetArchTest.TestStructure.Classes")
		        .And()
		        .HaveNameEndingWith("StaticClass")
		        .Should()
		        .BeStatic().GetResult();

	        Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are not static.")]
        public void AreNotStatic_MatchesFound_ClassesSelected()
        {
	        var result = Types
		        .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
		        .That()
		        .ResideInNamespace("NetArchTest.TestStructure.Classes")
		        .And()
		        .HaveName(nameof(ExampleClass))
		        .Should()
		        .NotBeStatic().GetResult();

	        Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are nested.")]
        public void AreNested_MatchesFound_ClassesSelected()
        {
            // Include both public and private nested classes
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nested")
                .And()
                .HaveNameEndingWith("Class")
                .Should()
                .BeNested().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are nested and public.")]
        public void AreNestedPublic_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nested")
                .And()
                .HaveName(typeof(NestedPublicClass).Name)
                .Should()
                .BeNestedPublic().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are nested and private.")]
        public void AreNestedPrivate_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nested")
                .And()
                .HaveName("NestedPrivateClass")
                .Should()
                .BeNestedPrivate().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are not nested.")]
        public void AreNotNested_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nested")
                .And()
                .HaveName(typeof(NotNested).Name)
                .Should()
                .NotBeNested().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are not nested and public.")]
        public void AreNotNestedPublic_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nested")
                .And()
                .HaveNameStartingWith("NestedPrivate")
                .Should()
                .NotBeNestedPublic().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are not nested.")]
        public void AreNotNestedPrivate_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nested")
                .And()
                .HaveNameStartingWith("NestedPublic")
                .Should()
                .NotBeNestedPrivate().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected for being declared as public.")]
        public void ArePublic_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Scope")
                .And()
                .HaveName("PublicClass")
                .Should()
                .BePublic().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected for not being declared as public.")]
        public void AreNotPublic_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Scope")
                .And()
                .DoNotHaveNameStartingWith("PublicClass")
                .Should()
                .NotBePublic().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected for being declared as sealed.")]
        public void AreSealed_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Scope")
                .And()
                .HaveName("SealedClass")
                .Should()
                .BeSealed().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected for not being declared as sealed.")]
        public void AreNotSealed_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Scope")
                .And()
                .DoNotHaveName("SealedClass")
                .Should()
                .NotBeSealed().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected for being immutable.")]
        public void AreImmutable_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Mutability")
                .And()
                .HaveNameStartingWith("ImmutableClass")
                .Should()
                .BeImmutable().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected for being mutable.")]
        public void AreMutable_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Mutability")
                .And()
                .DoNotHaveNameStartingWith("ImmutableClass")
                .Should()
                .BeMutable().GetResult();

            Assert.True(result.IsSuccessful);
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
                .DoNotHaveNameStartingWith("NonNullableClass")
                .Should()
                .OnlyHaveNullableMembers().GetResult();

            Assert.True(result.IsSuccessful);
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
                .DoNotHaveNameStartingWith("NullableClass")
                .Should()
                .HaveSomeNonNullableMembers().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they reside in a namespace.")]
        public void ResideInNamespace_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .HaveNameStartingWith("ClassA")
                .Should()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching").GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not reside in a namespace.")]
        public void NotResideInNamespace_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .HaveNameStartingWith("ClassA")
                .Should()
                .NotResideInNamespace("NetArchTest.TestStructure.Wrong").GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they reside in a namespace that matches a regular expression.")]
        public void ResideInNamespaceMatching_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(@"NetArchTest.TestStructure.NamespaceMatching")
                .Should()
                .ResideInNamespaceMatching(@"NetArchTest.TestStructure.NamespaceMatching.Namespace\w")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not reside in a namespace that matches a regular expression.")]
        public void NotResideInNamespaceMatching_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NamespaceMatching.NamespaceA")
                .Should()
                .NotResideInNamespaceMatching(@"NetArchTest.TestStructure.NamespaceMatching.Namespace\d")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they reside in a namespace that starts with a name part.")]
        public void ResideInNamespaceStartingWith_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .HaveNameStartingWith("ClassA")
                .Should()
                .ResideInNamespaceStartingWith("NetArchTest.TestStructure.NameMatching")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not reside in a namespace that start with name part.")]
        public void NotResideInNamespaceStartingWith_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespaceStartingWith("NetArchTest.TestStructure.NameMatching")
                .And()
                .HaveNameEndingWith("1")
                .Should()
                .NotResideInNamespaceStartingWith("NetArchTest.TestStructure.NameMatching.Namespace2")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they reside in a namespace that ends with a name part.")]
        public void ResideInNamespaceEndingWith_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .HaveName("ClassA1")
                .Should()
                .ResideInNamespaceEndingWith(".NameMatching.Namespace1")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not reside in a namespace that end with name part.")]
        public void NotResideInNamespaceEndingWith_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .HaveNameStartingWith("ClassA")
                .Should()
                .NotResideInNamespaceEndingWith(".Namespace3")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they reside in a namespace that contains a name part.")]
        public void ResideInNamespaceContaining_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .HaveNameStartingWith("ClassA")
                .Should()
                .ResideInNamespaceContaining(".NameMatching.")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not reside in a namespace that contains name part.")]
        public void NotResideInNamespaceContaining_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .HaveNameStartingWith("ClassA")
                .Should()
                .NotResideInNamespaceContaining("Namespace3")
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Selecting by namespace will return types in nested namespaces.")]
        public void ResideInNamespace_Nested_AllClassReturned()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .HaveName("ClassB2")
                .Should()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching").GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they have a dependency on a specific item.")]
        public void HaveDependency_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .HaveNameStartingWith("HasDepend")
                .Should()
                .HaveDependencyOn(typeof(ExampleDependency).FullName)
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they have a dependency on any item in a list.")]
        public void HaveDependencyOnAny_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .HaveNameStartingWith("Has")
                .Should()
                .HaveDependencyOnAny(new[] { typeof(ExampleDependency).FullName, typeof(AnotherExampleDependency).FullName })
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they have a dependency on all the items in a list.")]
        public void HaveDependencyOnAll_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .HaveNameStartingWith("HasDependencies")
                .Should()
                .HaveDependencyOnAll(new[] { typeof(ExampleDependency).FullName, typeof(AnotherExampleDependency).FullName })
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they only have a dependency on any item in a list.")]
        public void OnlyHaveDependenciesOn_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .HaveNameStartingWith("HasDependency")
                .Should()
                .OnlyHaveDependenciesOn(new[] { typeof(ExampleDependency).FullName, "System" })
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not have a dependency on another type.")]
        public void NotHaveDependency_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .HaveNameStartingWith("NoDependency")
                .Should()
                .NotHaveDependencyOn(typeof(ExampleDependency).FullName)
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not have a dependency on any item in a list.")]
        public void NotHaveDependencyOnAny_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .HaveNameStartingWith("NoDependency")
                .Should()
                .NotHaveDependencyOnAny(new[] { typeof(ExampleDependency).FullName, typeof(AnotherExampleDependency).FullName })
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they do not have a dependency on all the items in a list.")]
        public void NotHaveDependencyOnAll_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .HaveNameStartingWith("NoDependency")
                .Should()
                .NotHaveDependencyOnAll(new[] { typeof(ExampleDependency).FullName, typeof(AnotherExampleDependency).FullName })
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they have a dependency that is not on the a list.")]
        public void HaveDependenciesOtherThan_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace(typeof(HasDependency).Namespace)
                .And()
                .HaveNameStartingWith("HasDependencies")
                .Should()
                .HaveDependenciesOtherThan(new[] { typeof(ExampleDependency).FullName, "System" })
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types failing condition are reported when test fails.")]
        public void MatchNotFound_ClassesReported()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace1")
                .Should()
                .HaveName("ClassA2")
                .GetResult();

            Assert.False(result.IsSuccessful);

            var failingTypes = result.FailingTypes.ToList();
            Assert.Equal(2, failingTypes.Count);
            Assert.Equal("NetArchTest.TestStructure.NameMatching.Namespace1.ClassA1", failingTypes[0].ToString());
            Assert.Equal("NetArchTest.TestStructure.NameMatching.Namespace1.ClassB1", failingTypes[1].ToString());
        }

        [Fact(DisplayName = "Types can be selected according to a custom rule.")]
        public void MeetCustomRule_MatchesFound_ClassSelected()
        {
            // Create a custom rule that selected "ClassA1"
            var rule = new CustomRuleExample(t => t.Name.Equals("ClassA1", StringComparison.InvariantCultureIgnoreCase));

            // This rule uses the custom rule to confirm that "ClassA1" has been selected
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .HaveName("ClassA1")
                .Should()
                .MeetCustomRule(rule)
                .GetResult();

            // The custom rule selected the right class
            Assert.True(result.IsSuccessful);

            // The custom rule was executed at least once
            Assert.True(rule.TestMethodCalled);
        }

        [Fact(DisplayName = "Types can be selected if they are enumerations.")]
        public void AreEnums_MatchesFound_EnumsSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Enumerations")
                .And()
                .DoNotHaveName("NoEnum")
                .Should()
                .BeEnums().GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact(DisplayName = "Types can be selected if they are not enumerations.")]
        public void AreNotEnums_MatchesFound_EnumsSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Enumerations")
                .And()
                .HaveName("NoEnum")
                .Should()
                .NotBeEnums().GetResult();

            Assert.True(result.IsSuccessful);
        }
    }
}
