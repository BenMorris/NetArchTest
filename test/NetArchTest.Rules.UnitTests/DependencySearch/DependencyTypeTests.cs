namespace NetArchTest.Rules.UnitTests.DependencySearch
{
    using System;
    using System.Collections.Generic;
    using NetArchTest.TestStructure.Dependencies.Examples;
    using NetArchTest.TestStructure.Dependencies.Search.DependencyType;
    using Xunit;
    using Array = TestStructure.Dependencies.Search.DependencyType.Array;
    using Pointer = TestStructure.Dependencies.Search.DependencyType.Pointer;

    /// <summary>
    /// This tests collection verifies that dependency search checks every posible type.    
    /// </summary>
    [CollectionDefinition("Dependency Search - type tests ")]
    public class DependencyTypeTests
    {
        [Fact(DisplayName = "Finds a dependency ExampleDependency[] in ExampleDependency[].")]
        public void DependencySearch_Array_Found()
        {
            Utils.RunDependencyTest(typeof(Array), typeof(ExampleDependency[]), true, true);
        }

        [Fact(DisplayName = "Does not find a dependency ExampleDependency[].")]        
        public void DependencySearch_Array_NotFound()
        {
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(Array), typeof(ArrayJagged)),
                                    typeof(ExampleDependency[]), 
                                    false,
                                    true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency in ExampleDependency[].")]
        public void DependencySearch_ArrayNested_Found()
        {
            Utils.RunDependencyTest(typeof(Array));
        }        

        [Fact(DisplayName = "Finds a dependency ExampleDependency[][] in ExampleDependency[][].")]
        public void DependencySearch_ArrayJagged_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayJagged), typeof(ExampleDependency[][]), true, true);
        }

        [Fact(DisplayName = "Does not find a dependency ExampleDependency[][].")]       
        public void DependencySearch_ArrayJagged_NotFound()
        {           
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(ArrayJagged)),
                                    typeof(ExampleDependency[][]),
                                    false,
                                    true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency[] in ExampleDependency[][].")]
        public void DependencySearch_ArrayJaggedArray_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayJagged), typeof(ExampleDependency[]), true, true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency in ExampleDependency[][].")]
        public void DependencySearch_ArrayJaggedNested_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayJagged));
        }   

        [Fact(DisplayName = "Finds a dependency ExampleDependency[,,,] in ExampleDependency[,,,]. ")]
        public void DependencySearch_ArrayMultidimensional_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayMultidimensional), typeof(ExampleDependency[,,,]), true, true);
        }

        [Fact(DisplayName = "Does not find a dependency ExampleDependency[,,,].")]       
        public void DependencySearch_ArrayMultidimensional_NotFound()
        {         
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(ArrayMultidimensional)),
                                  typeof(ExampleDependency[,]),
                                  false,
                                  true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency in ExampleDependency[,,,].")]
        public void DependencySearch_ArrayMultidimensionalNested_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayMultidimensional));
        }
        
        [Fact(DisplayName = "Finds a dependency ExampleDependency<int>[] in ExampleDependency<int>[].")]
        public void DependencySearch_ArrayGeneric_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayOfGenerics), typeof(ExampleDependency<int>[]), true, true);
        }

        [Fact(DisplayName = "Does not find a dependency ExampleDependency<string>[].")]
        public void DependencySearch_ArrayGeneric_NotFound()
        {            
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(StaticGenericClass)),
                                  typeof(ExampleDependency<string>[]),
                                  false,
                                  true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency<int> in ExampleDependency<int>[].")]
        public void DependencySearch_ArrayGenericNested_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayOfGenerics), typeof(ExampleDependency<int>), true, true);
        }

        [Fact(DisplayName = "Does not find a dependency ExampleDependency<string>.")]
        public void DependencySearch_ArrayGenericNested_NotFound()
        {
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(StaticGenericClass)),
                                 typeof(ExampleDependency<string>),
                                 false,
                                 true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency<> in ExampleDependency<int>[].")]
        public void DependencySearch_ArrayGenericNestedOpen_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayOfGenerics), typeof(ExampleDependency<>), true, true);
        }

        [Fact(DisplayName = "Does not find a dependency ExampleDependency in ExampleDependency<int>[].")]
        public void DependencySearch_ArrayGenericNot_NotFound()
        {
            Utils.RunDependencyTest(typeof(ArrayOfGenerics), typeof(ExampleDependency), false, true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency in GenericClass<ExampleDependency>[].")]
        public void DependencySearch_ArrayOfGenericsTypeArgument_Found()
        {
            Utils.RunDependencyTest(typeof(ArrayOfGenericsTypeArgument));
        }

        [Fact(DisplayName = "Does not find a Dependency ExampleDependency[] in GenericClass<ExampleDependency>[].")]
        public void DependencySearch_ArrayOfGenericsTypeArgument_NotFound()
        {
            Utils.RunDependencyTest(typeof(ArrayOfGenericsTypeArgument), typeof(ExampleDependency[]), false, true);
        }        

        [Theory(DisplayName = "Finds a dependency ExampleDependency in method's parameter passed by reference.")]
        [InlineData(typeof(MethodParameterIn))]
        [InlineData(typeof(MethodParameterOut))]
        [InlineData(typeof(MethodParameterRef))]
        public void DependencySearch_MethodParameter_NotFound(Type input)
        {
            Utils.RunDependencyTest(input);
        }     
      
        [Fact(DisplayName = "Finds a NestedDependencyTree.NestedLevel1.NestedLevel2.NestedDependency in NestedDependencyTree.NestedLevel1.NestedLevel2.NestedDependency.")]
        public void DependencySearch_NestedDependencyClass_Found()
        {
            Utils.RunDependencyTest(typeof(NestedDependencyClass), typeof(NestedDependencyTree.NestedLevel1.NestedLevel2.NestedDependency), true, true);
        }

        [Fact(DisplayName = "Does not find a NestedDependencyTree.NestedLevel1.NestedLevel2.NestedDependency.")]
        public void DependencySearch_NestedDependency_NotFound()
        {
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(NestedDependencyClass)),
                              typeof(NestedDependencyTree.NestedLevel1.NestedLevel2.NestedDependency),
                              false,
                              true);
        }

        [Fact(DisplayName = "Finds a NestedDependencyTree.NestedLevel1.NestedLevel2.NestedDependency<int> in NestedDependencyTree.NestedLevel1.NestedLevel2.NestedDependency<int>.")]
        public void DependencySearch_NestedDependencyClassGeneric_Found()
        {
            Utils.RunDependencyTest(typeof(NestedDependencyClassGeneric), typeof(NestedDependencyTree.NestedLevel1.NestedLevel2.NestedDependency<int>), true, true);
        }

        [Fact(DisplayName = "Does not find a NestedDependencyTree.NestedLevel1.NestedLevel2.NestedDependency<int>.")]
        public void DependencySearch_NestedDependencyClassGeneric1_NotFound()
        {
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(NestedDependencyClassGeneric)),
                                 typeof(NestedDependencyTree.NestedLevel1.NestedLevel2.NestedDependency<int>),
                                 false,
                                 true);
        }

        [Fact(DisplayName = "Does not find a NestedDependencyTree.NestedLevel1.NestedLevel2.NestedDependency<double>.")]
        public void DependencySearch_NestedDependencyClassGeneric2_NotFound()
        {
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(StaticGenericClass)),
                                 typeof(NestedDependencyTree.NestedLevel1.NestedLevel2.NestedDependency<double>),
                                 false,
                                 true);
        }

        [Fact(DisplayName = "Finds a NestedDependencyTree.NestedLevel1.NestedLevel2<int>.NestedDependency<int> in NestedDependencyTree.NestedLevel1.NestedLevel2<int>.NestedDependency<int>.")]
        public void DependencySearch_NestedDependencyClassGenericLevel2Generic_Found()
        {
            Utils.RunDependencyTest(typeof(NestedDependencyClassGenericLevel2Generic), typeof(NestedDependencyTree.NestedLevel1.NestedLevel2<int>.NestedDependency<int>), true, true);
        }

        [Fact(DisplayName = "Does not find a NestedDependencyTree.NestedLevel1.NestedLevel2<int>.NestedDependency<int>")]
        public void DependencySearch_NestedDependencyClassGenericLevel2Generic1_NotFound()
        {
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(NestedDependencyClassGenericLevel2Generic)),
                                typeof(NestedDependencyTree.NestedLevel1.NestedLevel2<int>.NestedDependency<int>),
                                false,
                                true);
        }

        [Fact(DisplayName = "Does not find a NestedDependencyTree.NestedLevel1.NestedLevel2<int>.NestedDependency<double>")]
        public void DependencySearch_NestedDependencyClassGenericLevel2Generic2_NotFound()
        {
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(StaticGenericClass)),
                                typeof(NestedDependencyTree.NestedLevel1.NestedLevel2<int>.NestedDependency<double>),
                                false,
                                true);
        }

        [Fact(DisplayName = "Does not find a NestedDependencyTree.NestedLevel1.NestedLevel2<double>.NestedDependency<int>")]
        public void DependencySearch_NestedDependencyClassGenericLevel2Generic3_NotFound()
        {
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(StaticGenericClass)),
                                typeof(NestedDependencyTree.NestedLevel1.NestedLevel2<double>.NestedDependency<int>),
                                false,
                                true);
        }

        [Fact(DisplayName = "Finds a NestedDependencyTree.NestedLevel1.NestedLevel2<int>.NestedDependency in NestedDependencyTree.NestedLevel1.NestedLevel2<int>.NestedDependency.")]
        public void DependencySearch_NestedDependencyClassLevel2Generic_Found()
        {
            Utils.RunDependencyTest(typeof(NestedDependencyClassLevel2Generic), typeof(NestedDependencyTree.NestedLevel1.NestedLevel2<int>.NestedDependency), true, true);
        }

        [Fact(DisplayName = "Does not find a NestedDependencyTree.NestedLevel1.NestedLevel2<int>.NestedDependency.")]
        public void DependencySearch_NestedDependencyClassLevel2Generic1_NotFound()
        {
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(NestedDependencyClassLevel2Generic)),
                                 typeof(NestedDependencyTree.NestedLevel1.NestedLevel2<int>.NestedDependency),
                                 false,
                                 true);
        }

        [Fact(DisplayName = "Does not find a NestedDependencyTree.NestedLevel1.NestedLevel2<double>.NestedDependency")]
        public void DependencySearch_NestedDependencyClassLevel2Generic2_NotFound()
        {
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(StaticGenericClass)),
                                typeof(NestedDependencyTree.NestedLevel1.NestedLevel2<double>.NestedDependency),
                                false,
                                true);
        }       

        [Fact(DisplayName = "Finds a dependency StructDependency* in StructDependency*.")]
        public void DependencySearch_Pointer_Found()
        {
            Utils.RunDependencyTest(typeof(Pointer), typeof(StructDependency*), true, true);
        }

        [Fact(DisplayName = "Does not find a dependency StructDependency* in StructDependency.")]
        public void DependencySearch_Pointer_NotFound()
        {
            Utils.RunDependencyTest(typeof(PointerNot), typeof(StructDependency*), false, true);
        }

        [Fact(DisplayName = "Finds a dependency StructDependency in StructDependency*.")]
        public void DependencySearch_PointerNested_Found()
        {
            Utils.RunDependencyTest(typeof(Pointer), typeof(StructDependency), true, true);
        }

        [Fact(DisplayName = "Finds a dependency StaticGenericDependency<> in StaticGenericDependency<int>.")]       
        public void DependencySearch_StaticGenericClass_Found()
        {
            Utils.RunDependencyTest(typeof(StaticGenericClass), typeof(StaticGenericDependency<>), true, true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency in ExampleDependency.")]      
        public void DependencySearch_Variable_Found()
        {
            Utils.RunDependencyTest(typeof(Variable));
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency<> in ExampleDependency<int>.")]
        public void DependencySearch_VariableGeneric_Found()
        {
            Utils.RunDependencyTest(typeof(VariableGeneric), typeof(ExampleDependency<>), true, true);
        }

        [Fact(DisplayName = "Does not find a dependency ExampleDependency in ExampleDependency<>.")]
        public void DependencySearch_VariableGenericSimple_NotFound()
        {
            Utils.RunDependencyTest(typeof(VariableGeneric), typeof(ExampleDependency), false, true);
        }

        [Fact(DisplayName = "Does not find a dependency ExampleDependency<>.")]
        public void DependencySearch_VariableGeneric_NotFound()
        {
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(ArrayOfGenerics), typeof(VariableGeneric)),
                               typeof(ExampleDependency<>).MakeByRefType(),
                               false,
                               true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency<int> in ExampleDependency<int>.")]
        public void DependencySearch_VariableGenericClosed_Found()
        {
            Utils.RunDependencyTest(typeof(VariableGeneric), typeof(ExampleDependency<int>), true, true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency<int> in ExampleDependency<int>, specified as string (GenericDependency<int>)")]
        public void DependencySearch_VariableGenericAsString_Found()
        {
            Utils.RunDependencyTest(typeof(VariableGeneric), new[] { typeof(ExampleDependency<int>).ToString() }, true);
        }

        [Fact(DisplayName = "Does not find dependency ExampleDependency<string> in ExampleDependency<int>.")]
        public void DependencySearch_VariableGenericClosed_NotFound()
        {
            Utils.RunDependencyTest(typeof(VariableGeneric), typeof(ExampleDependency<string>), false, true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency in GenericClass<ExampleDependency>.")]
        public void DependencySearch_VariableGenericTypeArgument_Found()
        {
            Utils.RunDependencyTest(typeof(VariableGenericTypeArgument));
        }

        [Fact(DisplayName = "Does not find a dependency List<ExampleDependency> in GenericClass<ExampleDependency>.")]
        public void DependencySearch_VariableGenericTypeArgument_NotFound()
        {
            Utils.RunDependencyTest(typeof(VariableGenericTypeArgument), typeof(List<ExampleDependency>), false, false);
        }

        [Fact(DisplayName = "Finds a dependency GenericClass<GenericClass<ExampleDependency>> in GenericClass<GenericClass<ExampleDependency>>.")]
        public void DependencySearch_VariableGenericTypeArgumentNested_Found()
        {
            Utils.RunDependencyTest(typeof(VariableGenericTypeArgumentNested), typeof(GenericClass<GenericClass<ExampleDependency>>), true, true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency in GenericClass<GenericClass<ExampleDependency>>.")]
        public void DependencySearch_VariableGenericTypeArgumentNestedNested_Found()
        {
            Utils.RunDependencyTest(typeof(VariableGenericTypeArgumentNested));
        }        

        [Fact(DisplayName = "Finds a dependency ExampleDependency& in ExampleDependency&.")]
        public void DependencySearch_VariableRef_Found()
        {
            Utils.RunDependencyTest(typeof(VariableRef), typeof(ExampleDependency).MakeByRefType(), true, true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency in ExampleDependency&.")]
        public void DependencySearch_VariableRefNested_Found()
        {
            Utils.RunDependencyTest(typeof(VariableRef));
        }

        [Fact(DisplayName = "Does not find a dependency ExampleDependency&.")]
        public void DependencySearch_VariableRef_NotFound()
        {
            Utils.RunDependencyTest(Utils.GetTypesThatResideInTheSameNamespaceButWithoutGivenType(typeof(VariableRef), typeof(VariableRefGenericTypeArgument), typeof(MethodParameterIn), typeof(MethodParameterOut), typeof(MethodParameterRef)),
                                typeof(ExampleDependency).MakeByRefType(),
                                false,
                                true);
        }

        [Fact(DisplayName = "Finds a dependency GenericClass<ExampleDependency>& in GenericClass<ExampleDependency>&.")]
        public void DependencySearch_VariableRefGeneric_Found()
        {
            Utils.RunDependencyTest(typeof(VariableRefGenericTypeArgument), typeof(GenericClass<ExampleDependency>).MakeByRefType(), true, true);
        }

        [Fact(DisplayName = "Finds a dependency GenericClass<ExampleDependency>[]& in GenericClass<ExampleDependency>[]&.")]
        public void DependencySearch_VariableRefArrayOfGenericsTypeArgumentByRef_Found()
        {
            Utils.RunDependencyTest(typeof(VariableRefArrayOfGenericsTypeArgument), typeof(GenericClass<ExampleDependency>[]).MakeByRefType(), true, true);
        }

        [Fact(DisplayName = "Finds a dependency GenericClass<ExampleDependency>[] in GenericClass<ExampleDependency>[]&.")]
        public void DependencySearch_VariableRefArrayOfGenericsTypeArgument_Found()
        {
            Utils.RunDependencyTest(typeof(VariableRefArrayOfGenericsTypeArgument), typeof(GenericClass<ExampleDependency>[]), true, true);
        }
        
        [Fact(DisplayName = "Finds a dependency GenericClass<ExampleDependency> in GenericClass<ExampleDependency>[]&.")]
        public void DependencySearch_VariableRefArrayOfGenericsTypeArgumentNested_Found()
        {
            Utils.RunDependencyTest(typeof(VariableRefArrayOfGenericsTypeArgument), typeof(GenericClass<ExampleDependency>), true, true);
        }
       
        [Fact(DisplayName = "Finds a dependency Tuple<int, ExampleDependency> in Tuple<int, ExampleDependency>.")]
        public void DependencySearch_VariableTuple_Found()
        {
            Utils.RunDependencyTest(typeof(VariableTuple), typeof(Tuple<int, ExampleDependency>), true, true);
        }

        [Fact(DisplayName = "Finds a dependency ExampleDependency in Tuple<int, ExampleDependency>.")]
        public void DependencySearch_VariableTupleNested_Found()
        {
            Utils.RunDependencyTest(typeof(VariableTuple));
        }        

        [Fact(DisplayName = "Does not find a dependency Tuple<int, double> in Tuple<int, ExampleDependency>.")]
        public void DependencySearch_VariableTuple_NotFound()
        {
            Utils.RunDependencyTest(typeof(VariableTuple), typeof(Tuple<int, double>), false, true);
        }

        [Fact(DisplayName = "Finds a dependency Array in ConstStringFieldValue.")]
        public void DependencySearch_ConstFieldString_Found()
        {
            Utils.RunDependencyTest(typeof(ConstStringFieldValue), typeof(Array), true, true);
        }

        [Fact(DisplayName = "Does not find a dependency ArrayJagged in ConstStringFieldValue.")]
        public void DependencySearch_ConstFieldString_NotFound()
        {
            Utils.RunDependencyTest(typeof(ConstStringFieldValue), typeof(ArrayJagged), false, true);
        }
      
        [Fact(DisplayName = "Finds a dependency StaticType in BaseCtorCall.")]
        public void DependencySearch_BaseCtorCall_Found()
        {
            Utils.RunDependencyTest(typeof(BaseCtorCall), typeof(StaticType), true, true);
        }
    }
}