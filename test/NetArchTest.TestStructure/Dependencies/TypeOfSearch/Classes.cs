namespace NetArchTest.TestStructure.Dependencies.TypeOfSearch
{
    using System;
    using System.Collections.Generic;
    using System.Text;

#pragma warning disable 169

    public class Class_A
    {
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