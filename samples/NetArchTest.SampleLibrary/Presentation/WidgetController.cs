namespace NetArchTest.SampleLibrary.Presentation
{
    using System.Linq;
    using System.Threading.Tasks;
    using NetArchTest.SampleLibrary.Services;

    public class WidgetController
    {
        protected readonly IWidgetService _service;

        public WidgetController(IWidgetService service)
        {
            _service = service;
        }

        public async Task<string> ListSomeWidgetsAsync()
        {
            var widgets = await _service.GetWidgetsAsync();
            return (string.Join(", ", widgets.Select(w => w.Name)));
        }
    }
}
