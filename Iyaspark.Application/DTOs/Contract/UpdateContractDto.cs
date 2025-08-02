using Iyaspark.Domain.Enums;
using System;

namespace Iyaspark.Application.Modules.Contracts.DTOs
{
    public class UpdateContractDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RentType RentType { get; set; }

        public decimal? FixedRentAmount { get; set; }
        public decimal? RevenuePercentage { get; set; }

        public string? PdfFilePath { get; set; }

        public decimal? RevenueTarget { get; set; }
        public decimal? AboveTargetPercentage { get; set; }
        public decimal? BelowTargetPercentage { get; set; }

        public bool HasExtraStorage { get; set; }
        public decimal? ExtraStorageRent { get; set; }

        public string? ContractFilePath { get; set; } // opsiyonel dosya yolu güncelleme için
    }
}
