using Iyaspark.Domain.Entities;
using Iyaspark.Domain.Interfaces;
using Iyaspark.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Iyaspark.Persistence.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly AppDbContext _context;

        public ContractRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Contract contract)
        {
            await _context.Contracts.AddAsync(contract);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Contract>> GetByTenantIdAsync(Guid tenantId)
        {
            return await _context.Contracts
                .Where(c => c.TenantId == tenantId)
                .OrderByDescending(c => c.StartDate)
                .ToListAsync();
        }

        public async Task<Contract?> GetByIdAsync(Guid id)
        {
            return await _context.Contracts
                .Include(c => c.Tenant)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Contract>> GetAllAsync()
        {
            return await _context.Contracts
                .Include(c => c.Tenant)
                .OrderByDescending(c => c.StartDate)
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract != null)
            {
                _context.Contracts.Remove(contract);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Contract>> GetContractsInDateRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            return await _context.Contracts
                .Include(c => c.Tenant)
                .ThenInclude(t => t.MonthlyRevenues)
                .Where(c =>
                    (!startDate.HasValue || c.StartDate <= endDate) &&
                    (!endDate.HasValue || c.EndDate >= startDate))
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<Contract>> GetAllWithTenantAsync()
        {
            return await _context.Contracts.Include(c => c.Tenant).ToListAsync();
        }

        public async Task<bool> HasOverlappingContractAsync(Guid tenantId, DateTime startDate, DateTime endDate, Guid? excludeContractId)
        {
            return await _context.Contracts
                .AnyAsync(c =>
                    c.TenantId == tenantId &&
                    c.StartDate < endDate &&
                    c.EndDate > startDate &&
                    (!excludeContractId.HasValue || c.Id != excludeContractId.Value));
        }





    }
}
