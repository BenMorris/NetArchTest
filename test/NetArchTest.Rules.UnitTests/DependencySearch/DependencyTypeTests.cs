namespace NetArchTest.Rules.UnitTests.DependencySearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using NetArchTest.Rules.Dependencies;
    using NetArchTest.TestStructure.Dependencies.Examples;
    using NetArchTest.TestStructure.Dependencies.Search.DependencyType;
    using Xunit;
    using Array = TestStructure.Dependencies.Search.DependencyType.Array;

    /// <summary>
    /// This tests collection verifies that dependency search checks every posible type.    
    /// </summary>
    [CollectionDefinition("Dependency Search - type tests ")]
    public class DependencyTypeTests
    {
        [Fact(DisplayName = "Finds an array dependency (ExampleDependency[]).")]
        public void DependencySearch_Array_Found()
        {
            Utils.RunDependencyTest(typeof(Array), typeof(ExampleDependency[]), true, true);
        }

        [Theory(DisplayName = "Does not find an array dependency (ExampleDependency[]).")]
        [InlineData(typeof(ArrayMultidimensional))]
        [InlineData(typeof(Variable))]      
        public void DependencySearch_Array_NotFound(Type input)
        {
            Utils.RunDependencyTest(input, typeof(ExampleDependency[]), false, true);
        }

        [Fact(DisplayName = "Finds a dependency in array of dependencies (ExampleDependency[]).")]
        public void DependencySearch_ArraySingle_Found()
        {
            Utils.RunDependencyTest(typeof(Array));
        }

        [Fact(DisplayName = "Finds an array (jagged) dependency (ExampleDependency[][]).")]
        public void DependencySearch_ArrayJagged_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayJagged), typeof(ExampleDependency[][]), true, true);
        }

        [Fact(DisplayName = "Does not find an array (jagged) dependency (ExampleDependency[][]).")]
        public void DependencySearch_ArrayJagged_NotFound()
        {
            Utils.RunDependencyTest(typeof(ArrayMultidimensional), typeof(ExampleDependency[][]), false, true);
        }

        [Fact(DisplayName = "Finds a dependency in an array (jagged) of dependencies (ExampleDependency[][]).")]
        public void DependencySearch_ArrayJaggedSingle_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayJagged));
        }
        
        [Fact(DisplayName = "Finds an array (multidimensional) dependency (ExampleDependency[,,,]). ")]
        public void DependencySearch_ArrayMultidimensional_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayMultidimensional), typeof(ExampleDependency[,,,]), true, true);
        }

        [Fact(DisplayName = "Does not find an array (multidimensional) dependency (ExampleDependency[,,,]).")]
        public void DependencySearch_ArrayMultidimensional_NotFound()
        {
            Utils.RunDependencyTest(typeof(ArrayJagged), typeof(ExampleDependency[,]), false, true);
        }

        [Fact(DisplayName = "Finds a dependency in multidimensional array of dependencies (ExampleDependency[,,,]).")]
        public void DependencySearch_ArrayMultidimensionalSingle_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayMultidimensional));
        }

        [Fact(DisplayName = "Finds an array of closed generic dependecy (GenericDependecy<int>[]).")]
        public void DependencySearch_ArrayGeneric_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayOfGenerics), typeof(GenericDependecy<int>[]), true, true);
        }

        [Fact(DisplayName = "Does not find an array of generic closed dependecy (GenericDependecy<string>[]).")]
        public void DependencySearch_ArrayGeneric_NotFound()
        {
            Utils.RunDependencyTest(typeof(ArrayOfGenerics), typeof(GenericDependecy<string>[]), false, true);
        }

        [Fact(DisplayName = "Finds a dependency in an array of generic dependencies (GenericDependecy<int>).")]
        public void DependencySearch_ArrayGenericSingle_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayOfGenerics), typeof(GenericDependecy<int>), true, true);
        }               

        [Fact(DisplayName = "Finds a dependency in an array of generic dependencies (GenericClass<ExampleDependency>[]).")]
        public void DependencySearch_ArrayOfGenericsTypeArgument_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayOfGenericsTypeArgument));
        }

        [Fact(DisplayName = "Finds a disposable dependency in a using statement (DisposableDependency).")]
        public void DependencySearch_Disposable_Found()
        {
            Utils.RunDependencyTest(typeof(Disposable), typeof(DisposableDependency), true, true);
        }

        [Fact(DisplayName = "Finds a nested dependency.")]
        public void DependencySearch_NestedDependencyClass_Found()
        {
            Utils.RunDependencyTest(typeof(NestedDependencyClass), typeof(NestedDependencyTree.NestedLevel1.NestedDependency), true, true);
        }

        [Fact(DisplayName = "Does not find a nested generic dependency.")]
        public void DependencySearch_NestedDependency_NotFound()
        {
            Utils.RunDependencyTest(typeof(NestedDependencyClass), typeof(NestedDependencyTree.NestedLevel1<int>.NestedDependency), false, true);
        }

        [Fact(DisplayName = "Finds a dependency nested in generic.")]
        public void DependencySearch_NestedInGenericDependency_Found()
        {
            Utils.RunDependencyTest(typeof(NestedInGenericDependency), typeof(NestedDependencyTree.NestedLevel1<int>.NestedDependency), true, true);
        }

        [Fact(DisplayName = "Does not find a dependency nested in generic.")]
        public void DependencySearch_NestedInGenericDependency_NotFound()
        {
            Utils.RunDependencyTest(typeof(NestedInGenericDependency), typeof(NestedDependencyTree.NestedLevel1.NestedDependency), false, true);
        }

        [Fact(DisplayName = "Finds a dependency in a static generic class.")]       
        public void DependencySearch_StaticGenericClass_Found()
        {
            Utils.RunDependencyTest(typeof(StaticGenericClass), typeof(StaticGenericDependency<>), true, true);
        }

        [Fact(DisplayName = "Finds a dependency in a variable (ExampleDependency).")]      
        public void DependencySearch_Variable_Found()
        {
            Utils.RunDependencyTest(typeof(Variable));
        }       

        [Fact(DisplayName = "Finds a dependency in a variable (List<ExampleDependency>).")]
        public void DependencySearch_VariableGenericTypeArgument_Found()
        {
            Utils.RunDependencyTest(typeof(VariableGenericTypeArgument));
        }

        [Fact(DisplayName = "Finds a dependency in a variable (List<List<ExampleDependency>>).")]
        public void DependencySearch_VariableGenericTypeArgumentNested_Found()
        {
            Utils.RunDependencyTest(typeof(VariableGenericTypeArgumentNested));
        }

        [Fact(DisplayName = "Finds a dependency in a variable (Tuple<int, ExampleDependency>).")]
        public void DependencySearch_VariableTuple_Found()
        {
            Utils.RunDependencyTest(typeof(VariableTuple));
        }

        [Fact(DisplayName = "Finds a generic open dependecy (GenericDependecy<>).")]
        public void DependencySearch_VariableGeneric_Found()
        {
            Utils.RunDependencyTest(typeof(VariableGeneric), typeof(GenericDependecy<>), true, true);
        }

        [Fact(DisplayName = "Does not find a depenedncy in a generic open dependecy (GenericDependecy<>).")]
        public void DependencySearch_VariableGeneric_Not()
        {
            Utils.RunDependencyTest(typeof(VariableGeneric), typeof(GenericDependecy), false, true);
        }

        [Fact(DisplayName = "Finds a generic closed dependecy (GenericDependecy<int>).")]
        public void DependencySearch_VariableGenericClosed_Found()
        {
            Utils.RunDependencyTest(typeof(VariableGeneric), typeof(GenericDependecy<int>), true, true);
        }

        [Fact(DisplayName = "Finds a generic closed dependecy, specified as string (GenericDependecy<int>)")]
        public void DependencySearch_VariableGenericAsString_Found()
        {
            Utils.RunDependencyTest(typeof(VariableGeneric), new[] { typeof(GenericDependecy<int>).ToString() }, true);
        }

        [Fact(DisplayName = "Does not find a generic closed dependecy (GenericDependecy<string>).")]
        public void DependencySearch_VariableGenericClosed_NotFound()
        {
            Utils.RunDependencyTest(typeof(VariableGeneric), typeof(GenericDependecy<string>), false, true);
        }        

        [Fact(DisplayName = "Finds a generic closed nested dependecy (List<List<ExampleDependency>>).")]
        public void DependencySearch_GenericClosedNested_Found()
        {
            Utils.RunDependencyTest(typeof(VariableGenericTypeArgumentNested), typeof(List<List<ExampleDependency>>), true, true);
        }

        [Fact(DisplayName = "Does not find a generic closed nested dependecy (List<List<int>>).")]
        public void DependencySearch_GenericClosedNested_NotFound()
        {
            Utils.RunDependencyTest(typeof(VariableGenericTypeArgumentNested), typeof(List<List<int>>), false, true);
        }

        [Fact(DisplayName = "Finds a generic closed tuple dependecy (Tuple<int, ExampleDependency>).")]
        public void DependencySearch_GenericClosedDouble_Found()
        {
            Utils.RunDependencyTest(typeof(VariableTuple), typeof(Tuple<int, ExampleDependency>), true, true);
        }

        [Fact(DisplayName = "Does not find a generic closed tuple dependecy (Tuple<int, double>).")]
        public void DependencySearch_GenericClosedDouble_NotFound()
        {
            Utils.RunDependencyTest(typeof(VariableTuple), typeof(Tuple<int, double>), false, true);
        }        
    }
}