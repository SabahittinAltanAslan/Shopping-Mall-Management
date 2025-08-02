public class CreateMonthlyRevenueDto
{
    public Guid TenantId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal RevenueAmount { get; set; }
    public CurrencyType CurrencyType { get; set; }

}