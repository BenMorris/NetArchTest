namespace NetArchTest.Rules
{    
    /// <summary>
    /// Executor of condition.
    /// </summary>
    public interface ISliceConditionList
    {
        /// <summary>
        /// Returns an indication of whether all the selected types satisfy the conditions.
        /// </summary>
        /// <returns>An indication of whether the conditions are true, along with a list of types failing the check if they are not.</returns>

        ITestResult GetResult();
    }
}
