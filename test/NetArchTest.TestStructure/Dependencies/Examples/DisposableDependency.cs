using System;

namespace NetArchTest.TestStructure.Dependencies.Examples
{
    /// <summary>
    /// An example class used in tests that identify dependencies.
    /// </summary>
    class DisposableDependency : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
