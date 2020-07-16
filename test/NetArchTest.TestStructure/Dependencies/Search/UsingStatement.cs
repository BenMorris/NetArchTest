using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NetArchTest.TestStructure.Dependencies.Examples;

namespace NetArchTest.TestStructure.Dependencies.Search
{
    class UsingStatement
    {
        public UsingStatement()
        {
            using(var foo = new DisposableDependency())
            {

            }
        }
    }
}
