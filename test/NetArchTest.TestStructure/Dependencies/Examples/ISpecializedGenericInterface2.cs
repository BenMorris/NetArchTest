using System;
using System.Collections.Generic;

namespace NetArchTest.TestStructure.Dependencies.Examples
{
	interface ISpecializedGenericInterface2<T1, T2, T3> : IGenericInterface<T1, T2>
		where T1 : IEquatable<T1>
		where T2 : IEnumerable<T1>
	{

	}
}