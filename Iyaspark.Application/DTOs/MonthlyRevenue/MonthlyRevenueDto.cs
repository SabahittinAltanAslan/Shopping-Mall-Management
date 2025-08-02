public class MonthlyRevenueDto
{
    public int Id { get; set; }
    public Guid TenantId { get; set; }
    public string CompanyName { get; set; }
    public string TaxNumber { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal RevenueAmount { get; set; }
    public CurrencyType CurrencyType { get; set; }

}