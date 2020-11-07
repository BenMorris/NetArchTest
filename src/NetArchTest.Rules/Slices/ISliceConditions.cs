namespace NetArchTest.Rules
{
    /// <summary>
    /// A set of conditions that can be applied to slices of types.
    /// </summary>
    public interface ISliceConditions
    {
        /// <summary>
        /// Selects types that do not have dependencies on types from other slices.
        /// </summary>        
        ISliceConditionList NotHaveDependenciesBetweenSlices();


        /// <summary>
        /// Selects types that have some dependencies on types from other slices.
        /// </summary>  
        ISliceConditionList HaveDependenciesBetweenSlices();
    }
}