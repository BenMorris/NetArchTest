namespace NetArchTest.Rules.UnitTests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using NetArchTest.Rules.Extensions;
    using NetArchTest.TestStructure.Abstract;
    using Xunit;

    public class FunctionSequenceTests
    {
        [Fact(DisplayName = "Setting the Selected flag to false will return the types that fail the sequence.")]
        public void Execute_SelectedFalse_ReturnsFailedTypes()
        {
            // Arrange
            var sequence = new FunctionSequence();
            sequence.AddFunctionCall(FunctionDelegates.BeAbstract, true, true);

            var types = Types
            .InAssembly(Assembly.GetAssembly(typeof(AbstractClass)))
            .That()
            .ResideInNamespace("NetArchTest.TestStructure.Abstract")
            .GetTypeDefinitions();

            // Act
            var resultSelected = sequence.Execute(types).Select(t => t.ToType());
            var resultNotSelected = sequence.Execute(types, selected: false).Select(t => t.ToType());

            // Assert
            // The default behaviour is to return the types that were selected
            Assert.Single(resultSelected);
            Assert.Contains<Type>(typeof(AbstractClass), resultSelected);

            // Setting the flag to false will return the types that were NOT selected
            Assert.Single(resultNotSelected);
            Assert.Contains<Type>(typeof(ConcreteClass), resultNotSelected);
        }
    }
}
