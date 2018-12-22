using NetArchTest.Rules.Matches;
using static NetArchTest.Rules.Matchers;

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
                .That(ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3"))
                .Should(HaveName("ClassB2"))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they do not have a specific name.")]
        public void NotHaveName_MatchFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace1"))
                .Should(!HaveName("ClassB2"))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected by the start of their name.")]
        public void HaveNameStarting_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.NameMatching"))
                .Should(HaveNameStartingWith("Class"))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if their name does not have a specific start.")]
        public void NotHaveNameStarting_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.NameMatching"))
                .Should(!HaveNameStartingWith("X"))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected by the end of their name.")]
        public void HaveNameEnding_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2.Namespace3"))
                .Should(HaveNameEndingWith("B2"))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if their name does not have a specific end.")]
        public void NotHaveNameEnding_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.NameMatching.Namespace2.Namespace1"))
                .Should(!HaveNameEndingWith("B2"))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected by a regular expression.")]
        public void HaveNameMatching_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.NameMatching"))
                .Should(HaveNameMatching(@"Class\w\d"))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they do not conform to a regular expression.")]
        public void NotHaveNameMatching_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.NameMatching"))
                .Should(!HaveNameMatching(@"X\w"))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected by a the presence of a custom attribute.")]
        public void HaveCustomAttribute_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.CustomAttributes") 
                      & HaveName("ClassAttributePresent"))                
                .Should(HaveCustomAttribute(typeof(ClassCustomAttribute)))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected by the absence of a custom attribute.")]
        public void NotHaveCustomAttribute_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.CustomAttributes") & !HaveName("AttributePresent"))
                .Should(!HaveCustomAttribute(typeof(ClassCustomAttribute)))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they inherit from a type.")]
        public void Inherit_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Inheritance")
                    & HaveNameStartingWith("Derived"))
                .Should(Inherit(typeof(BaseClass)))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they do not inherit from a type.")]
        public void NotInherit_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Inheritance")
                    & !HaveNameStartingWith("Derived"))
                .Should(!Inherit(typeof(BaseClass)))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they implement an interface.")]
        public void ImplementInterface_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Interfaces")
                & HaveNameStartingWith("Implements"))
                .Should(ImplementInterface(typeof(IExample)))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they do not implement an interface.")]
        public void NotImplementInterface_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Interfaces")
                & !HaveNameStartingWith("Implements"))
                .Should(!ImplementInterface(typeof(IExample)))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they are abstract.")]
        public void AreAbstract_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Abstract")
                & HaveNameStartingWith("Abstract"))                
                .Should(BeAbstract())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they are not abstract.")]
        public void AreNotAbstract_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Abstract")
                & !HaveNameStartingWith("Abstract"))
                .Should(!BeAbstract())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they are classes.")]
        public void AreClasses_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Classes")
                & HaveNameEndingWith("Class"))                
                .Should(BeClass())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they are not classes.")]
        public void AreNotClasses_MatchesFound_ClassesSelected ()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Classes") & 
                      HaveNameEndingWith("Interface"))
                .Should(! BeClass())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they have generic parameters.")]
        public void AreGeneric_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Generic") & 
                      HaveNameStartingWith("Generic"))
                .Should(BeGeneric())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they do not have generic parameters.")]
        public void AreNotGeneric_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Generic") & 
                      HaveNameStartingWith("NonGeneric"))
                .Should(! BeGeneric())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they are interfaces.")]
        public void AreInterfaces_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Classes")
                & HaveNameEndingWith("Interface"))                
                .Should( BeInterfaces() )
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they are not interfaces.")]
        public void AreNotInterfaces_MatchesFound_ClassesSelected ()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Classes")
                & HaveNameEndingWith("Class"))                
                .Should(! BeInterfaces())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they are nested.")]
        public void AreNested_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Nested")
                & HaveName("NestedClass"))
                .Should(BeNested())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they are not nested.")]
        public void AreNotNested_MatchesFound_ClassesSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Nested")
                & !HaveName("NestedClass"))
                .Should(! BeNested())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected for being declared as public.")]
        public void ArePublic_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Scope")
                & HaveName("PublicClass"))
                .Should(BePublic())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected for not being declared as public.")]
        public void AreNotPublic_MatchesFound_ClassSelected ()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Scope")
                & !HaveName("PublicClass"))
                .Should(! BePublic())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected for being declared as sealed.")]
        public void AreSealed_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Scope")
                & HaveName("SealedClass"))
                .Should(BeSealed())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected for not being declared as sealed.")]
        public void AreNotSealed_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Scope")
                    & !HaveName("SealedClass"))
                .Should(!BeSealed())
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they reside in a namespace.")]
        public void ResideInNamespace_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(HaveNameStartingWith("ClassA"))
                .Should(
                    ResideInNamespace("NetArchTest.TestStructure.NameMatching")
                ).GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they do not reside in a namespace.")]
        public void NotResideInNamespace_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(HaveNameStartingWith("ClassA"))
                .Should(! ResideInNamespace("NetArchTest.TestStructure.Wrong"))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Selecting by namespace will return types in nested namespaces.")]
        public void ResideInNamespace_Nested_AllClassReturned()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(HaveName("ClassB2"))
                .Should(ResideInNamespace("NetArchTest.TestStructure.NameMatching"))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they have a dependency on another type.")]
        public void HaveDepencency_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(ResideInNamespace("NetArchTest.TestStructure.Dependencies.Implementation")
                & HaveNameStartingWith("HasDependency"))
                .Should(HaveDependencyOn("NetArchTest.TestStructure.Dependencies.ExampleDependency"))
                .GetResult();

            Assert.True(result);
        }

        [Fact(DisplayName = "Types can be selected if they do not have a dependency on another type.")]
        public void NotHaveDependency_MatchesFound_ClassSelected()
        {
            var result = Types
                .InAssembly(Assembly.GetAssembly(typeof(ClassA1)))
                .That(
                    ResideInNamespace("NetArchTest.TestStructure.Dependencies.Implementation") 
                    & HaveNameStartingWith("NoDependency")
                    & HaveDependencyOn("NetArchTest.TestStructure.Dependencies.ExampleDependency")
                    )
                .Should(
                    HaveDependencyOn("NetArchTest.TestStructure.Dependencies.ExampleDependency")
                ).GetResult();

            Assert.True(result);
        }
    }
}
