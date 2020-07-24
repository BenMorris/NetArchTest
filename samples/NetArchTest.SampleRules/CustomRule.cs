namespace NetArchTest.SampleRules
{
    using Mono.Cecil;
    using NetArchTest.Rules;

    /// <summary>
    /// A custom rule that can be used to extend NetArchTest.
    /// </summary>
    public class CustomRule : ICustomRule
    {
        /// <remarks>
        /// This method will be executed against every type - so take care over external look-ups, etc
        /// </remarks>
        public bool MeetsRule(TypeDefinition type)
        {
            return true;
        }
    }
}
