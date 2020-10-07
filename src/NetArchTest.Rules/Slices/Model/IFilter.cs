namespace NetArchTest.Rules.Slices.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    interface IFilter
    {
        IEnumerable<TypeResult> Execute(SlicedTypes slices);
    }
}
