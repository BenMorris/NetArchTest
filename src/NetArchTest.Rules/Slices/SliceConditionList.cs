namespace NetArchTest.Rules.Slices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetArchTest.Rules.Slices.Model;
   

    internal sealed class SliceConditionList : ISliceConditionList
    {
        private readonly IFilter _filter;
        private readonly SlicedTypes _slicedTypes;
        private readonly bool _should;

        public SliceConditionList(IFilter filter, SlicedTypes slicedTypes, bool should)
        {
            _filter = filter;
            _slicedTypes = slicedTypes;
            _should = should;
        }



        public ITestResult GetResult()
        {
            var filteredTypes = _filter.Execute(_slicedTypes);

            if (filteredTypes.Count() != _slicedTypes.TypeCount)
            {
                throw new Exception("Filter returned wrong number of results!");
            }

            bool successIsWhen = _should;
            bool isSuccessful = filteredTypes.All(x => x.IsPassing == successIsWhen);           

            if (isSuccessful)
            {
                return TestResult.Success();
            }
            else
            {
                var failingTypes = filteredTypes.Where(x => x.IsPassing == !successIsWhen);
                return TestResult.Failure(failingTypes);
            }
        }
    }
}
