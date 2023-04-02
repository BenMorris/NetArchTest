namespace NetArchTest.TestStructure.Dependencies.Search.DependencyLocation
{
    using System.Runtime.InteropServices;
    using NetArchTest.TestStructure.Dependencies.Examples;

    /// <summary>
    /// Example class that includes a dependency in a P/Invoke.    
    /// </summary>
    public class PInvoke
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int MessageBox(ExampleDependency hWnd);


       
    }
}
