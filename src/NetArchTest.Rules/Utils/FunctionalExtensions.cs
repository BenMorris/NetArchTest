using System;

namespace NetArchTest.Rules
{
    public static class FunctionalExtensions
    {
        public static Func<T, bool> Reverse<T>(Func<T, bool> func)
        {
            return t => !func(t);
        }
        
        public static Func<T, bool> And<T>(Func<T, bool> left, Func<T, bool> right)
        {
            return t => left(t) && right(t);
        }
        
        public static Func<T, bool> Or<T>(Func<T, bool> left, Func<T, bool> right)
        {
            return t => left(t) || right(t);
        }
    }
}