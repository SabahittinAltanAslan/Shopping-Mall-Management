using Iyaspark.Domain.Entities;

namespace Iyaspark.Domain.Interfaces
{
    public interface IContractRepository
    {
        Task AddAsync(Contract contract);
        Task<List<Contract>> GetByTenantIdAsync(Guid tenantId);
        Task<Contract?> GetByIdAsync(Guid id);
        Task<List<Contract>> GetAllAsync(); // artık Tenant include edilerek dönüyor
        Task DeleteAsync(Guid id);
        Task SaveAsync();
        Task<List<Contract>> GetContractsInDateRangeAsync(DateTime? startDate, DateTime? endDate);
        Task<List<Contract>> GetAllWithTenantAsync();
        Task<bool> HasOverlappingContractAsync(Guid tenantId, DateTime startDate, DateTime endDate, Guid? excludeContractId);




    }
}
