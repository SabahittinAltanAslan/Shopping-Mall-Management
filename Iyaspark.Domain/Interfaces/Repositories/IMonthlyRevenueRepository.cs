using Iyaspark.Domain.Entities;
using System.Linq.Expressions;

namespace Iyaspark.Domain.Interfaces
{
    public interface IMonthlyRevenueRepository
    {
        Task<List<MonthlyRevenue>> GetAllAsync();
        Task<List<MonthlyRevenue>> GetByTenantIdAsync(Guid tenantId);
        Task<MonthlyRevenue?> GetByIdAsync(int id);
        Task AddAsync(MonthlyRevenue revenue);
        Task UpdateAsync(MonthlyRevenue revenue);
        Task DeleteAsync(int id);
        Task SaveAsync();
        Task<List<MonthlyRevenue>> GetAllWithTenantAsync();
        Task<List<MonthlyRevenue>> GetAllByMonthYearAsync(int year, int month);
        Task<bool> ExistsAsync(Guid tenantId, int year, int month, int? excludeId = null);

        Task<bool> AnyAsync(Expression<Func<MonthlyRevenue, bool>> predicate);





    }
}
