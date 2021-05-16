namespace NetArchTest.Rules.Slices
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NetArchTest.Rules.Slices.Model;

    internal sealed class SliceList : ISliceList
    {
        private readonly SlicedTypes _slicedTypes;

        public SliceList(SlicedTypes slicedTypes)
        {
            _slicedTypes = slicedTypes;
        }

        public ISliceConditions Should()
        {
            return new SliceConditions(_slicedTypes, true);
        }

        public ISliceConditions ShouldNot()
        {
            return new SliceConditions(_slicedTypes, false);
        }


        public SlicedTypes GetSlicedTypes()
        {
            return _slicedTypes;
        }
    }
}