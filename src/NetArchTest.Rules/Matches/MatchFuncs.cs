using System;

namespace NetArchTest.Rules.Matches
{
    internal static class MatchFuncs
    {
        public static Func<T, bool> Exclude<T>(Func<T, bool> left, Func<T, bool> right)
        {
            return t => left(t) && !right(t);
        }
        public static Func<T, bool> Include<T>(Func<T, bool> left, Func<T, bool> right)
        {
            return t => left(t) || right(t);
        }
    }
}