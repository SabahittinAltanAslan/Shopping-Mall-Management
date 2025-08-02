using Iyaspark.Domain.Entities;

namespace Iyaspark.Domain.Interfaces
{
    public interface ITenantRepository
    {
        Task<List<Tenant>> GetAllAsync();
        Task<Tenant?> GetByIdAsync(Guid id);
        Task AddAsync(Tenant tenant);
        Task UpdateAsync(Tenant tenant);
        Task DeleteAsync(Guid id);
    }
}
