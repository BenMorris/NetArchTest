namespace NetArchTest.SampleLibrary.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> ListAsync();
    }
}
