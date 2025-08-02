namespace Iyaspark.Application.Modules.Reports.DTOs
{
    public class ProfitabilityReportDto
    {
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
        public string Sector { get; set; }
        public string FacadeDirection { get; set; }
        public string TenantType { get; set; }
        public string? FloorCode { get; set; }
        public string FloorLabel { get; set; }
        public decimal? SquareMeter { get; set; }
        public string RentType { get; set; }

        public decimal? FixedRentAmount { get; set; }
        public decimal? RevenueBasedRent { get; set; }
        public decimal? ExtraStorageRent { get; set; }

        public decimal TotalRevenue { get; set; }  // Kiracının ciro bilgisi
        public decimal TotalRent { get; set; }     // AVM'nin toplam kira geliri
        public decimal Profit => TotalRent;        // Direkt gelir bizim için kâr
    }
}
