using Iyaspark.Domain.Enums;

namespace Iyaspark.Domain.Entities
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
        public int SquareMeter { get; set; }
        public string FloorCode { get; set; }
        public string FloorLabel { get; set; }

        public Sector Sector { get; set; }
        public FacadeDirection FacadeDirection { get; set; }
        public TenantType TenantType { get; set; }

        public bool HasExtraStorage { get; set; }

        public ICollection<MonthlyRevenue> MonthlyRevenues { get; set; } = new List<MonthlyRevenue>();

    }
}
