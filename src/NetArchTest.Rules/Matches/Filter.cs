using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace NetArchTest.Rules.Matches
{
    public class Filter 
    {
        private readonly Func<IEnumerable<TypeDefinition>, IEnumerable<TypeDefinition>> _filterFunc;

        public Filter(Func<IEnumerable<TypeDefinition>, IEnumerable<TypeDefinition>> filterFunc)
        {
            _filterFunc = filterFunc;
        }
        
        public static implicit operator Func<IEnumerable<TypeDefinition>, IEnumerable<TypeDefinition>>(Filter filter)
        {
            return filter._filterFunc;
        }
        
        public static Filter operator &(Filter left, Filter right)
        {
            return new Filter(input => right._filterFunc(left._filterFunc(input)));
        }
        
        public static Filter operator |(Filter left, Filter right)
        {
            return new Filter(input => right._filterFunc(input).Concat(left._filterFunc(input)).Distinct());
        }
        
        public static Filter operator !(Filter filter)
        {
            return new Filter(input => input.Except(filter._filterFunc(input)));
        }

    }
}