using Iyaspark.Domain.Enums;

public class MonthlyRentDto
{
    public string CompanyName { get; set; }
    public string TaxNumber { get; set; }
    public string FloorLabel { get; set; }
    public string FacadeDirection { get; set; }
    public string Sector { get; set; }
    public string RentType { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal RentAmount { get; set; }
    public TenantType TenantType { get; set; }

}
