namespace Iyaspark.Application.Modules.Dashboard.DTOs
{
    public class SectorProfitComparisonDto
    {
        public string Sector { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalRent { get; set; }
        public decimal Profit => TotalRent;
    }
}
