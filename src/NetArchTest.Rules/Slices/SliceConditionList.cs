namespace NetArchTest.Rules.Slices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetArchTest.Rules.Slices.Model;
   

    internal sealed class SliceConditionList : ISliceConditionList
    {
        private readonly IFilter _filter;
        private readonly SlicedTypes _slices;
        private readonly bool _should;

        public SliceConditionList(IFilter filter, SlicedTypes slices, bool should)
        {
            _filter = filter;
            _slices = slices;
            _should = should;
        }



        public ITestResult GetResult()
        {
            var filterResults = _filter.Execute(_slices);

            bool successIsWhen = _should;
            bool success = filterResults.All(x => x.IsPassing == successIsWhen);

            if (filterResults.Count() != _slices.TypeCount)
            {
                throw new Exception("Filter returned wrong number of results!");
            }

            if (success)
            {
                return TestResult.Success();
            }
            else
            {
                var failingTypes = filterResults.Where(x => x.IsPassing == !successIsWhen);
                return TestResult.Failure(failingTypes);
            }
        }
    }
}
