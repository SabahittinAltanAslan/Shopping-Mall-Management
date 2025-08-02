namespace Iyaspark.Application.Modules.Reports.DTOs
{
    public class RevenueReportDto
    {
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
        public string Sector { get; set; }
        public string FacadeDirection { get; set; }
        public string TenantType { get; set; }
        public string FloorCode { get; set; }
        public string FloorLabel { get; set; }
        public decimal SquareMeter { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}
