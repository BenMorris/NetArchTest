using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using static NetArchTest.Rules.FunctionalExtensions;

namespace NetArchTest.Rules.Matches
{
    public class Matcher
    {
        private readonly Func<TypeDefinition, bool> _matchFunc;

        public Matcher(Func<TypeDefinition, bool> matchFunc)
        {
            _matchFunc = matchFunc;
        }

        public static implicit operator Func<TypeDefinition, bool>(Matcher matcher)
        {
            return matcher._matchFunc;
        }
        
        public static Matcher operator &(Matcher left, Matcher right)
        {
            return new Matcher(And(left._matchFunc, right._matchFunc));
        }
        
        public static Matcher operator |(Matcher left, Matcher right)
        {
            return new Matcher(Or(left._matchFunc, right._matchFunc));
        }
        
        public static Matcher operator !(Matcher matcher)
        {
            return new Matcher(Reverse(matcher._matchFunc));
        }
        
        public static implicit operator Filter(Matcher matcher)
        {
            return new Filter(input => input.Where(matcher._matchFunc));
        }

    }
    
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