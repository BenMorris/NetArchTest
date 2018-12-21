﻿using NetArchTest.Rules.Matches;

namespace NetArchTest.Rules.UnitTests
{
    using System.Reflection;
    using NetArchTest.TestStructure.CustomAttributes;
    using NetArchTest.TestStructure.Dependencies;
    using NetArchTest.TestStructure.Inheritance;
    using NetArchTest.TestStructure.Interfaces;
    using NetArchTest.TestStructure.NameMatching.Namespace1;
    using Xunit;

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

            Assert.True(result);
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

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected by the start of their name.")]
        public void HaveNameStarting_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .Should()
                .HaveNameStartingWith("Class").GetResult();

            Assert.True(result);
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

            Assert.True(result);
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

            Assert.True(result);
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

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected by a regular expression.")]
        public void HaveNameMatching_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                .Should()
                .HaveNameMatching(@"Class\w\d").GetResult();

            Assert.True(result);
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

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected by a the presence of a custom attribute.")]
        public void HaveCustomAttribute_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.CustomAttributes")
                .And()
                .HaveName("ClassAttributePresent")
                .Should()
                .HaveCustomAttribute(typeof(ClassCustomAttribute)).GetResult();

            Assert.True(result);
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

            Assert.True(result);
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

            Assert.True(result);
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

            Assert.True(result);
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

            Assert.True(result);
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

            Assert.True(result);
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

            Assert.True(result);
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

            Assert.True(result);
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

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they are not classes.")]
        public void AreNotClasses_MatchesFound_ClassesSelected ()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Classes")
                .And()
                .HaveNameEndingWith("Interface")
                .Should()
                .NotBeClasses().GetResult();

            Assert.True(result);
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

            Assert.True(result);
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

            Assert.True(result);
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

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they are not interfaces.")]
        public void AreNotInterfaces_MatchesFound_ClassesSelected ()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Classes")
                .And()
                .HaveNameEndingWith("Class")
                .Should()
                .NotBeInterfaces().GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they are nested.")]
        public void AreNested_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nested")
                .And()
                .HaveName("NestedClass")
                .Should()
                .BeNested().GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they are not nested.")]
        public void AreNotNested_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Nested")
                .And()
                .DoNotHaveName("NestedClass")
                .Should()
                .NotBeNested().GetResult();

            Assert.True(result);
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

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected for not being declared as public.")]
        public void AreNotPublic_MatchesFound_ClassSelected ()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Scope")
                .And()
                .DoNotHaveName("PublicClass")
                .Should()
                .NotBePublic().GetResult();

            Assert.True(result);
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

            Assert.True(result);
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

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they reside in a namespace.")]
        public void ResideInNamespace_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .HaveNameStartingWith("ClassA")
                .Should().ResideInNamespace(Globbing.New("NetArchTest.TestStructure.NameMatching.*")).GetResult();

            Assert.True(result);
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

            Assert.True(result);
        }

        [Fact(DisplayName = "Selecting by namespace will return types in nested namespaces.")]
        public void ResideInNamespace_Nested_AllClassReturned()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .HaveName("ClassB2")
                .Should().ResideInNamespace(Globbing.New("NetArchTest.TestStructure.NameMatching.*")).GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they have a dependency on another type.")]
        public void HaveDepencency_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Dependencies.Implementation")
                .And()
                .HaveNameStartingWith("HasDependency")
                .Should()
                .HaveDependencyOn("NetArchTest.TestStructure.Dependencies.ExampleDependency")
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they do not have a dependency on another type.")]
        public void NotHaveDepencency_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That()
                .ResideInNamespace("NetArchTest.TestStructure.Dependencies.Implementation")
                .And()
                .HaveNameStartingWith("NoDependency")
                .Should()
                .NotHaveDependencyOn("NetArchTest.TestStructure.Dependencies.ExampleDependency")
                .GetResult();

            Assert.True(result);
        }
    }
}
