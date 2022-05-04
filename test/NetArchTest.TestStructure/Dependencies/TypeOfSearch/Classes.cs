namespace NetArchTest.TestStructure.Dependencies.TypeOfSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

#pragma warning disable 169

    public class Class_A
    {
        string stringField = "I am not a dependency!";

        public static void LetUsCreateSomeAnonymousTypes()
        {
            var numbers = Enumerable.Range(0, 1);
            var result = from x in numbers
                         join z in numbers on x equals z
                         select (x, z);
        }


        public static int LetUsUseSwitchExpressionWithMoreThan7(string element)
        {            
            return element switch
            {
                "one" => 1,
                "two" => 2,
                "three" => 3,
                "four" => 4,
                "five" => 5,
                "six" => 6,
                "seven" => 7,
                _ => 8,
            };
        }
    }

    public class Class_B  
    {
        Dependency_3 dp3;
    }

    public class Class_C 
    {      
        Dependency_2 dp2;      
    }

    public class Class_D
    {
        Dependency_2 dp2;
        Dependency_3 dp3;
    }

    public class Class_E
    {    
        Dependency_1 dp1;      
    }

    public class Class_F
    {
        Dependency_1 dp1;       
        Dependency_3 dp3;
    }

    public class Class_G
    {
        Dependency_1 dp1;
        Dependency_2 dp2;       
    }

    public class Class_H
    {
        Dependency_1 dp1;
        Dependency_2 dp2;
        Dependency_3 dp3;
    }

#pragma warning restore 169

}