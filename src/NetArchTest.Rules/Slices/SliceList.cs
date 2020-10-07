namespace NetArchTest.Rules.Slices
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NetArchTest.Rules.Slices.Model;

    internal sealed class SliceList : ISliceList
    {
        private readonly SlicedTypes _slices;

        public SliceList(SlicedTypes slices)
        {
            _slices = slices;
        }

        public ISliceConditions Should()
        {
            return new SliceConditions(_slices, true);
        }

        public ISliceConditions ShouldNot()
        {
            return new SliceConditions(_slices, false);
        }
    }
}