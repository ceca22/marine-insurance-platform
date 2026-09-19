
namespace InsuranceApp.Services.Interfaces
{
    public interface IService<T>
    {
        Task<T> CreateAsync(T entity);
        Task<bool> DeleteEntityAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id); 
    }
}
