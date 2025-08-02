using Iyaspark.Domain.Entities;
using Iyaspark.Domain.Interfaces;
using Iyaspark.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Iyaspark.Infrastructure.Repositories
{
    public class GuaranteeRepository : IGuaranteeRepository
    {
        private readonly AppDbContext _context;

        public GuaranteeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Guarantee guarantee)
        {
            await _context.Guarantees.AddAsync(guarantee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guarantee guarantee)
        {
            _context.Guarantees.Update(guarantee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Guarantees.FindAsync(id);
            if (entity != null)
            {
                _context.Guarantees.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Guarantee?> GetByIdAsync(Guid id)
        {
            return await _context.Guarantees
                .Include(g => g.Tenant)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<List<Guarantee>> GetAllWithTenantAsync()
        {
            return await _context.Guarantees
                .Include(g => g.Tenant)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
