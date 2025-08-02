using BCrypt.Net;
using DocumentFormat.OpenXml.Math;
using Iyaspark.Domain.Entities;
using Iyaspark.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Iyaspark.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<MonthlyRevenue> MonthlyRevenues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Guarantee> Guarantees { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }


    }
}
