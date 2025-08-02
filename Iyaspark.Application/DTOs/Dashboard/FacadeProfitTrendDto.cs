namespace Iyaspark.Application.Modules.Dashboard.DTOs
{
    public class FacadeProfitTrendDto
    {
        public string FacadeDirection { get; set; }
        public decimal ThisMonthProfit { get; set; }
        public decimal LastMonthProfit { get; set; }
        public decimal ChangePercentage { get; set; } // Negatifse düşüş
    }
}
