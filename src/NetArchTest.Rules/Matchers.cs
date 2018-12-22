using System;
using System.Linq;
using System.Text.RegularExpressions;
using NetArchTest.Rules.Dependencies;
using NetArchTest.Rules.Extensions;
using NetArchTest.Rules.Matches;
using NetArchTest.Rules.Utils;

namespace NetArchTest.Rules
{
    public static partial class Matchers
    {
        public static Matcher ResideInNamespace(string @namespace)
        {
            return new Matcher(c => c.GetNamespace().StartsWith(@namespace));
        }
        
        public static Matcher ResideInNamespace(Func<string, bool> match)
        {
            return new Matcher(t => match(t.Namespace));
        }
        
        public static Matcher HaveNameStartingWith(string start)
        {
            return new Matcher(c => c.Name.StartsWith(start));
        }
        
        public static Matcher HaveNameEndingWith(string end)
        {
            return new Matcher(c => c.Name.EndsWith(end));
        }
        
        public static Filter HaveDependencyOn(string name)
        {
            return HaveDependencyOn(Globbing.New(name));
        }
        
        public static Filter HaveDependencyOn(Func<string, bool> match)
        {
            return new Filter(input => 
                new DependencySearch(match)
                    .FindTypesWithDependenciesMatch(input.ToList())
                    .GetResults());
        }

        public static Matcher BeInterfaces()
        {
            return new Matcher(c => c.IsInterface);
        }
        
        public static Matcher AreInterfaces()
        {
            return BeInterfaces();
        }
        
        public static Matcher BeClass()
        {
            return new Matcher(c => c.IsClass);
        }
        
        public static Matcher AreClasses()
        {
            return BeClass();
        }

        public static Matcher BeSealed()
        {
            return new Matcher(c => c.IsSealed);
        }
        
        public static Matcher AreSealed()
        {
            return BeSealed();
        }
        
        public static Matcher BePublic()
        {
            return new Matcher(c => c.IsPublic);
        }
        
        public static Matcher ArePublic()
        {
            return BePublic();
        }

        public static Matcher BeNested()
        {
            return new Matcher(c => c.IsNested);
        }
        
        public static Matcher AreNested()
        {
            return BeNested();
        }
        
        public static Matcher BeGeneric()
        {
            return new Matcher(c => c.HasGenericParameters);
        }
        
        public static Matcher AreGeneric()
        {
            return BeGeneric();
        }
        
        public static Matcher BeAbstract()
        {
            return new Matcher(c => c.IsAbstract);
        }
        
        public static Matcher AreAbstract()
        {
            return BeAbstract();
        }

        public static Filter ImplementInterface(Type interfaceType)
        {
            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException($"The type {interfaceType.FullName} is not an interface.");
            }

            return new Filter(input => input.Where(type => type.Interfaces
                .Any(t => t.InterfaceType.FullName == interfaceType.FullName)));
        }
        
        public static Matcher HaveNameMatching(string pattern)
        {
            Regex r = new Regex(pattern);
            return new Matcher(c => r.Match(c.Name).Success); 
        }
        
        public static Matcher HaveCustomAttribute(Type attribute)
        {
            return new Matcher(c => c.CustomAttributes.Any(a => attribute.FullName == a.AttributeType.FullName));
        }

        public static Matcher HaveName(string name)
        {
            return new Matcher(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
        public static Matcher Inherit(Type type)
        {
            var target = type.ToTypeDefinition();
            return new Matcher(c => c.IsSubclassOf(target));
        }
    }
}