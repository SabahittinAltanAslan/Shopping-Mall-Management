namespace Iyaspark.Application.DTOs.Dashboard
{
    public class FloorProfitDto
    {
        public string FloorLabel { get; set; } // Zemin, 1, 2, -1
        public decimal TotalRevenue { get; set; }
        public decimal TotalRent { get; set; }
        public decimal Profit => TotalRent;
    }
}
