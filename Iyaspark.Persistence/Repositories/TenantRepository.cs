using Iyaspark.Domain.Entities;
using Iyaspark.Domain.Interfaces;
using Iyaspark.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Iyaspark.Persistence.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext _context;

        public TenantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Tenant tenant)
        {
            await _context.Tenants.AddAsync(tenant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant != null)
            {
                _context.Tenants.Remove(tenant);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Tenant>> GetAllAsync()
        {
            return await _context.Tenants.ToListAsync();
        }

        public async Task<Tenant?> GetByIdAsync(Guid id)
        {
            return await _context.Tenants.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateAsync(Tenant tenant)
        {
            _context.Tenants.Update(tenant);
            await _context.SaveChangesAsync();
        }
    }
}
