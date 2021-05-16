namespace NetArchTest.Rules
{
    /// <summary>
    /// Link between predicate and condition.
    /// </summary>
    public interface ISliceList
    {
        /// <summary>
        /// Links a predicate defining a set of classes to a condition that tests them.
        /// </summary>
        ISliceConditions Should();

        /// <summary>
        /// Links a predicate defining a set of classes to a condition that tests them.
        /// </summary>
        ISliceConditions ShouldNot();
    }
}