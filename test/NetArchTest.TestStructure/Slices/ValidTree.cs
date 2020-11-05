namespace NetArchTest.TestStructure.Slices
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Text;
    

    namespace ValidTree
    {
        namespace FeatureA
        {
            using NetArchTest.TestStructure.Slices.ValidTree.FeatureA.App;
            namespace App
            {
                using NetArchTest.TestStructure.Slices.ValidTree.FeatureA.Domain;

                class AppService { AggregateRoot root; }
            }
            namespace Domain
            {
                class ValueObject { }
                class AggregateRoot { ValueObject value; }
            }
            namespace Infrastructure
            {
                namespace Persistence 
                {
                    using NetArchTest.TestStructure.Slices.ValidTree.FeatureA.Domain;
                    class Repository {  AggregateRoot root; }
                }
            }
            class FeatureA : FeatureBase { AppService app; }
        }
        namespace FeatureB
        {
            class AppService { DAO dao;  }
            class DAO { }
            class FeatureB : FeatureBase { AppService app; }
        }
        namespace FeatureC
        {            
            class FeatureC : FeatureBase { }
        }

        class FeatureBase
        {

        }
    }
}
