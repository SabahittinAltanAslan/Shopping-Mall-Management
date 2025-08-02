using Iyaspark.Domain.Entities;
using Iyaspark.Domain.Interfaces;
using Iyaspark.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Iyaspark.Persistance.Repositories
{
    public class MonthlyRevenueRepository : IMonthlyRevenueRepository
    {
        private readonly AppDbContext _context;

        public MonthlyRevenueRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MonthlyRevenue revenue)
        {
            await _context.MonthlyRevenues.AddAsync(revenue);
        }

        public async Task<List<MonthlyRevenue>> GetAllAsync()
        {
            return await _context.MonthlyRevenues
                .Include(x => x.Tenant)
                .ToListAsync();
        }

        public async Task<List<MonthlyRevenue>> GetByTenantIdAsync(Guid tenantId)
        {
            return await _context.MonthlyRevenues
                .Where(x => x.TenantId == tenantId)
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .ToListAsync();
        }

        public async Task<MonthlyRevenue?> GetByIdAsync(int id)
        {
            return await _context.MonthlyRevenues.FindAsync(id);
        }

        public Task UpdateAsync(MonthlyRevenue revenue)
        {
            _context.MonthlyRevenues.Update(revenue);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.MonthlyRevenues.FindAsync(id);
            if (entity != null)
            {
                _context.MonthlyRevenues.Remove(entity);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<MonthlyRevenue>> GetAllWithTenantAsync()
        {
            return await _context.MonthlyRevenues
                .Include(x => x.Tenant)
                .ToListAsync();
        }
        public async Task<List<MonthlyRevenue>> GetAllByMonthYearAsync(int year, int month)
        {
            return await _context.MonthlyRevenues
                .Include(x => x.Tenant)
                .Where(x => x.Year == year && x.Month == month)
                .ToListAsync();
        }
        public async Task<bool> ExistsAsync(Guid tenantId, int year, int month, int? excludeId = null)
        {
            return await _context.MonthlyRevenues.AnyAsync(x =>
                x.TenantId == tenantId &&
                x.Year == year &&
                x.Month == month &&
                (excludeId == null || x.Id != excludeId));
        }

        public async Task<bool> AnyAsync(Expression<Func<MonthlyRevenue, bool>> predicate)
        {
            return await _context.MonthlyRevenues.AnyAsync(predicate);
        }





    }
}
