using System;
using System.Linq;
using Mono.Cecil;
using static NetArchTest.Rules.Utils.FunctionalExtensions;

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
}