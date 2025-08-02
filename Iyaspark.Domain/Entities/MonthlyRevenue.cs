using System;

namespace Iyaspark.Domain.Entities
{
    public class MonthlyRevenue
    {
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public int Month { get; set; } // 1-12
        public int Year { get; set; }
        public decimal RevenueAmount { get; set; }
        public CurrencyType CurrencyType { get; set; }  // Enum


        public Tenant Tenant { get; set; } // Navigation
    }
}
