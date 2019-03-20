namespace NetArchTest.TestStructure.Nullable {
    
    /// <summary>
    /// An example class that has has non-nullable (i.e.null simple value typed) members.
    /// </summary>
    public class NonNullableClass3 {
        public TestEnum EnumProperty {get; set;}

        public enum TestEnum {
            red, 
            blue, 
            green
        }
    }
}