namespace NetArchTest.SampleLibrary.Services
{
    using NetArchTest.SampleLibrary.Data;
    using NetArchTest.SampleLibrary.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class WidgetService : IWidgetService
    {
        private readonly IRepository<Widget> _repository;

        public WidgetService(IRepository<Widget> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Widget>> GetWidgetsAsync()
        {
            return await _repository.ListAsync();
        }
    }
}
