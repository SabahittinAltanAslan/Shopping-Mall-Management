namespace Iyaspark.Application.Modules.MonthlyRevenues.DTOs
{
    public class UpdateMonthlyRevenueDto
    {
        public Guid TenantId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double RevenueAmount { get; set; }
        public int CurrencyType { get; set; }
    }
}
