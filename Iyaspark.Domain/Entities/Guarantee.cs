using Iyaspark.Domain.Enums;
using Iyaspark.SharedKernel.Entities;

namespace Iyaspark.Domain.Entities
{
    public class Guarantee : BaseEntity
    {
        public Guid TenantId { get; set; }
        public GuaranteeType Type { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public DateTime ReceivedDate { get; set; }
        public bool IsReturned { get; set; }
        public string? Description { get; set; }

        public Tenant Tenant { get; set; }
    }
}
