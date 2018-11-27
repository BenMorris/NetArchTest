namespace NetArchTest.SampleLibrary.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NetArchTest.SampleLibrary.Model;

    public class WidgetRepository : IRepository<Widget>, IDisposable
    {
        public async Task<IEnumerable<Widget>> ListAsync()
        {
            // Dummy reference used to demonstrate a dependency on System.Data
            var table = new System.Data.DataTable();

            return await Task.FromResult<IEnumerable<Widget>>(
                new List<Widget>
                {
                    new Widget{ Id = 1, Name = "Widget 1", Invalid = false },
                    new Widget{ Id = 2, Name = "Widget 2", Invalid = true }
                });
        }

        public void Dispose()
        {
        }
    }
}
