namespace NetArchTest.Rules
{
    public interface ISliceList
    {
        ISliceConditions Should();
        ISliceConditions ShouldNot();
    }
}