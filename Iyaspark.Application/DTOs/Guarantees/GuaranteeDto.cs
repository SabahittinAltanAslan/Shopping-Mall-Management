using Iyaspark.Domain.Enums;

namespace Iyaspark.Application.Modules.Guarantees.DTOs
{
    public class GuaranteeDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string CompanyName { get; set; } // Navigation'dan alınacak
        public GuaranteeType Type { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public decimal Amount { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string? Description { get; set; }
        public bool IsReturned { get; set; }
    }
}
