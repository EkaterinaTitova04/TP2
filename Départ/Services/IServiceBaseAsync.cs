using JuliePro.Models;

namespace JuliePro.Services
{
    public interface IServiceBaseAsync<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task DeleteAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task EditAsync(T entity);
        Task<List<Trainer>> GetFilteredTrainersAsync(TrainerSearchViewModelFilter filter);
        Task<List<string>> GetCertificationCentersAsync();
    }
}
