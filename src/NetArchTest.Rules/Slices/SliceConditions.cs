namespace NetArchTest.Rules.Slices
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NetArchTest.Rules.Slices.Model;


    internal sealed class SliceConditions : ISliceConditions
    {
        private readonly SlicedTypes _slicedTypes;
        private readonly bool _should;


        public SliceConditions(SlicedTypes slices, bool should)
        {
            _slicedTypes = slices;
            _should = should;
        }


        public ISliceConditionList HaveDependenciesBetweenSlices()
        {
            return new SliceConditionList(new HaveDependenciesBetweenSlices(), _slicedTypes, _should);
        }

        public ISliceConditionList NotHaveDependenciesBetweenSlices()
        {
            return new SliceConditionList(new HaveDependenciesBetweenSlices(), _slicedTypes, !_should);
        }
    }
}
