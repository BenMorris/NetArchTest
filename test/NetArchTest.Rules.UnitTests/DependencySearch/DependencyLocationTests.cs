namespace NetArchTest.Rules.UnitTests.DependencySearch
{
    using System;
    using NetArchTest.TestStructure.Dependencies.Examples;
    using NetArchTest.TestStructure.Dependencies.Search.DependencyLocation;
    using Xunit;

    /// <summary>
    /// This tests collection verifies that dependency search checks every posible place in C# code.
    /// It search for ExampleDependency, when in given place there is no posiblity of using ExampleDependency
    /// then it looks for: AttributeDependency or ExceptionDependency 
    /// </summary>
    [CollectionDefinition("Dependency Search - location tests ")]
    public class DependencyLocationTests
    {
        [Fact(DisplayName = "Finds a dependency in an async method.")]
        public void DependencySearch_AsyncMethod_Found()
        {
            Utils.RunDependencyTest(typeof(AsyncMethod));
        }

        [Theory(DisplayName = "Finds a dependency in an attribute.")]
        [InlineData(typeof(AttributeOnClass))]
        [InlineData(typeof(AttributeOnEvent))]
        [InlineData(typeof(AttributeOnField))]
        [InlineData(typeof(AttributeOnMethod))]
        [InlineData(typeof(AttributeOnParameter))]
        [InlineData(typeof(AttributeOnProperty))]
        [InlineData(typeof(AttributeOnReturnValue))]
        public void DependencySearch_Attribute_Found(Type input)
        {
            Utils.RunDependencyTest(input, typeof(AttributeDependency), true, true);
        }

        [Fact(DisplayName = "Finds a dependency in a private constructor.")]
        public void DependencySearch_ConstructorPrivate_Found()
        {
            Utils.RunDependencyTest(typeof(ConstructorPrivate));
        }

        [Fact(DisplayName = "Finds a dependency in a public constructor.")]
        public void DependencySearch_ConstructorPublic_Found()
        {
            Utils.RunDependencyTest(typeof(ConstructorPublic));
        }

        [Fact(DisplayName = "Finds a dependency in a default interface method body.")]
        public void DependencySearch_DefaultInterfaceMethodBody_Found()
        {
            Utils.RunDependencyTest(typeof(DefaultInterfaceMethodBody));
        }

        [Fact(DisplayName = "Finds a dependency in a delegate declaration.")]
        public void DependencySearch_DelegateDeclaration_Found()
        {
            Utils.RunDependencyTest(typeof(DelegateDeclaration));
        }

        [Fact(DisplayName = "Finds a dependency in a public event's add accessor.")]
        public void DependencySearch_EventAdd_Found()
        {
            Utils.RunDependencyTest(typeof(EventAdd));
        }

        [Fact(DisplayName = "Finds a dependency in a public event.")]
        public void DependencySearch_EventPublic_Found()
        {
            Utils.RunDependencyTest(typeof(EventPublic));
        }

        [Fact(DisplayName = "Finds a dependency in a public event's remove accessor.")]
        public void DependencySearch_EventRemove_Found()
        {
            Utils.RunDependencyTest(typeof(EventRemove));
        }

        [Fact(DisplayName = "Finds a dependency in a private field.")]
        public void DependencySearch_FieldPrivate_Found()
        {
            Utils.RunDependencyTest(typeof(FieldPrivate));
        }

        [Fact(DisplayName = "Finds a dependency in a public field.")]       
        public void DependencySearch_FieldPublic_Found()
        {
            Utils.RunDependencyTest(typeof(FieldPublic));
        }

        [Fact(DisplayName = "Finds a dependency in a generic class constraint.")]
        public void DependencySearch_GenericConstraintClass_Found()
        {
            Utils.RunDependencyTest(typeof(GenericConstraintClass<>));
        }

        [Fact(DisplayName = "Finds a dependency in a generic method constraint.")]
        public void DependencySearch_GenericConstraintMethod_Found()
        {
            Utils.RunDependencyTest(typeof(GenericConstraintMethod));
        }

        [Theory(DisplayName = "Finds a dependency in a generic type argument.")]
        [InlineData(typeof(GenericMethodTypeArgument))]
        [InlineData(typeof(GenericMethodTypeArgumentOneOpenOneClosedTypeArgument<>))]
        public void DependencySearch_GenericMethodTypeArgument_Found(Type input)
        {
            Utils.RunDependencyTest(input);
        }

        [Fact(DisplayName = "Finds a dependency in an implemented interface.")]     
        public void DependencySearch_ImplementedInterface_Found()
        {
            Utils.RunDependencyTest(typeof(ImplementedInterface));
        }

        [Fact(DisplayName = "Finds a dependency in a public indexer.")]
        public void DependencySearch_IndexerPublic_Found()
        {
            Utils.RunDependencyTest(typeof(IndexerPublic));
        }

        [Fact(DisplayName = "Finds a dependency in an inherited base class.")]      
        public void DependencySearch_InheritedBaseClass_Found()
        {
            Utils.RunDependencyTest(typeof(InheritedBaseClass));
        }

        [Theory(DisplayName = "Finds a dependency in an instruction invocation.")]
        [InlineData(typeof(InstructionCtor))]
        [InlineData(typeof(InstructionStaticClassTypeArgument))]
        [InlineData(typeof(InstructionStaticMethodTypeArgument))]        
        public void DependencySearch_Instruction_Found(Type input)
        {
            Utils.RunDependencyTest(input);
        }

        [Fact(DisplayName = "Finds a dependency in an instruction invocation (throw).")]    
        public void DependencySearch_InstructionThrow_Found()
        {
            Utils.RunDependencyTest(typeof(InstructionThrow), typeof(ExceptionDependency), true, true);
        }

        [Fact(DisplayName = "Finds a dependency in a captured variable by lambda closure.")]
        public void DependencySearch_LambdaCapturedVariable_Found()
        {
            Utils.RunDependencyTest(typeof(LambdaCapturedVariable));
        }

        [Fact(DisplayName = "Finds a dependency in a method's argument.")]         
        public void DependencySearch_MethodArgument_Found()
        {
            Utils.RunDependencyTest(typeof(MethodArgument));
        }

        [Fact(DisplayName = "Finds a dependency in a method's parameter.")]        
        public void DependencySearch_MethodParameter_Found()
        {
            Utils.RunDependencyTest(typeof(MethodParameter));
        }

        [Fact(DisplayName = "Finds a dependency in a private method's body.")]
        public void DependencySearch_MethodPrivateBody_Found()
        {
            Utils.RunDependencyTest(typeof(MethodPrivateBody));
        }

        [Fact(DisplayName = "Finds a dependency in a method's return type.")]         
        public void DependencySearch_MethodReturnType_Found()
        {
            Utils.RunDependencyTest(typeof(MethodReturnType));
        }

        [Fact(DisplayName = "Finds a dependency in a private property.")]
        public void DependencySearch_PropertyPrivate_Found()
        {
            Utils.RunDependencyTest(typeof(PropertyPrivate));
        }

        [Fact(DisplayName = "Finds a dependency in a public property.")]     
        public void DependencySearch_PropertyPublic_Found()
        {
            Utils.RunDependencyTest(typeof(PropertyPublic));
        }

        [Fact(DisplayName = "Finds a dependency in a P/Invoke.")]
        public void DependencySearch_PInvoke_Found()
        {
            Utils.RunDependencyTest(typeof(PInvoke));
        }

        [Fact(DisplayName = "Finds a dependency in a property getter.")]
        public void DependencySearch_PropertyGetter_Found()
        {
            Utils.RunDependencyTest(typeof(PropertyGetter));
        }

        [Fact(DisplayName = "Finds a dependency in a property setter.")]
        public void DependencySearch_PropertySetter_Found()
        {
            Utils.RunDependencyTest(typeof(PropertySetter));
        }

        [Fact(DisplayName = "Finds a dependency in a static local function body.")]
        public void DependencySearch_StaticLocalFunctions_Found()
        {
            Utils.RunDependencyTest(typeof(StaticLocalFunction));
        }

        [Fact(DisplayName = "Finds a dependency in a switch pattern matching.")]
        public void DependencySearch_SwitchPatternMatching_Found()
        {
            Utils.RunDependencyTest(typeof(SwitchPatternMatching));
        }

        [Fact(DisplayName = "Finds a dependency in a catch statement.")]
        public void DependencySearch_TryCatch_Found()
        {
            Utils.RunDependencyTest(typeof(TryCatch), typeof(ExceptionDependency), true, true);
        }

        [Fact(DisplayName = "Finds a dependency in a catch block.")]
        public void DependencySearch_TryCatchBlock_Found()
        {
            Utils.RunDependencyTest(typeof(TryCatchBlock), typeof(ExceptionDependency), true, true);
        }

        [Fact(DisplayName = "Finds a dependency in an exception filter.")]
        public void DependencySearch_TryCatchExceptionFilter_Found()
        {
            Utils.RunDependencyTest(typeof(TryCatchExceptionFilter), typeof(ExceptionDependency), true, true);
        }

        [Fact(DisplayName = "Finds a dependency in a finally block.")]
        public void DependencySearch_TryFinallyBlock_Found()
        {
            Utils.RunDependencyTest(typeof(TryFinallyBlock));
        }

        [Fact(DisplayName = "Finds a dependency in using statement.")]
        public void DependencySearch_UsingStatement_Found()
        {
            Utils.RunDependencyTest(typeof(UsingStatement), typeof(DisposableDependency), true, true);
        }

        [Fact(DisplayName = "Finds a dependency in a yield return statement.")]
        public void DependencySearch_Yield_Found()
        {
            Utils.RunDependencyTest(typeof(Yield));
        }        
    }
}