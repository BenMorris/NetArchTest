using System;
using System.Collections.Generic;
using System.Text;

namespace NetArchTest.TestStructure.Dependencies.Examples
{
    public static class Factory
    {
        private static ExampleDependency dependency;
        private static GenericClass<ExampleDependency> dependencyGeneric;
        private static GenericClass<ExampleDependency>[] dependencyArrayOfGenerics;
        private static GenericClass<double> dependencyGenericDouble;
        private static GenericClass<double>[] dependencyArrayOfDoubles;

        public static ref ExampleDependency Create()
        {
            return ref dependency;
        }


        public static ref GenericClass<double> CreateGenericDouble()
        {
            return ref dependencyGenericDouble;
        }

        public static ref GenericClass<ExampleDependency> CreateGeneric()
        {
            return ref dependencyGeneric;
        }
        public static ref GenericClass<ExampleDependency>[] CreateArrayOfGenerics()
        {
            return ref dependencyArrayOfGenerics;
        }
        public static ref GenericClass<double>[] CreateArrayOfDoubles()
        {
            return ref dependencyArrayOfDoubles;
        }
    }
}
