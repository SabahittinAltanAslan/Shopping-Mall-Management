using Iyaspark.Domain.Enums;

namespace Iyaspark.Application.Modules.Guarantees.DTOs
{
    public class CreateGuaranteeDto
    {
        public Guid TenantId { get; set; }
        public GuaranteeType Type { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string? Description { get; set; }
        public bool IsReturned { get; set; }
    }
}
