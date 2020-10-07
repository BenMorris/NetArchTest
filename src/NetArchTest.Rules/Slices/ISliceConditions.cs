namespace NetArchTest.Rules
{
    public interface ISliceConditions
    {
        ISliceConditionList NotHaveDependenciesBetweenSlices();

        ISliceConditionList HaveDependenciesBetweenSlices();
    }
}