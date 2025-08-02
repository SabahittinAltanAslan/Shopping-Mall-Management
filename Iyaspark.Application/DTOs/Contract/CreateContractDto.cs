using Iyaspark.Domain.Enums;

namespace Iyaspark.Application.DTOs.Contract
{
    public class CreateContractDto
    {
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
        public string? ContractFilePath { get; set; }

        public string? PdfFilePath { get; set; } // Eğer upload desteklenirse
    }
}
