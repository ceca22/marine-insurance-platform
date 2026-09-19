
namespace InsuranceApp.DataAccess.Interfaces
{
    public interface IRepository<T>
    {
        Task<T?> GetAsync(string id);
        Task<List<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}