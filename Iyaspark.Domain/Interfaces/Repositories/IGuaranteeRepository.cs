using Iyaspark.Domain.Entities;

namespace Iyaspark.Domain.Interfaces
{
    public interface IGuaranteeRepository
    {
        Task AddAsync(Guarantee guarantee);
        Task UpdateAsync(Guarantee guarantee);
        Task DeleteAsync(Guid id);
        Task<Guarantee> GetByIdAsync(Guid id);
        Task<List<Guarantee>> GetAllWithTenantAsync(); // Tenant ile birlikte getir
        Task SaveChangesAsync(); // Varsa UnitOfWork
    }
}
