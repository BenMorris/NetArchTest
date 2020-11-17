using System;
using System.Collections.Generic;
using System.Text;

namespace NetArchTest.TestStructure.Dependencies.Examples
{
    interface IGenericInterface<T>
    {
    }

    interface IGenericInterface<T1, T2>
        where T1 : IEquatable<T1>
        where T2 : IEnumerable<T1>
    {

    }
}
