using Iyaspark.Domain.Enums;

namespace Iyaspark.Domain.Entities
{
    public class Contract
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public RentType RentType { get; set; }

        public decimal? FixedRentAmount { get; set; }
        public double? RevenuePercentage { get; set; }

        public decimal? RevenueTarget { get; set; }
        public double? AboveTargetPercentage { get; set; }
        public double? BelowTargetPercentage { get; set; }

        public decimal? ExtraStorageRent { get; set; }
        public decimal? MonthlyDues { get; set; }

        public string? PdfFilePath { get; set; }

        public Tenant Tenant { get; set; } // Navigation
    }
}
