using Iyaspark.Application.DTOs;
using MediatR;

namespace Iyaspark.Application.Modules.MonthlyRevenue.Queries
{
    public class GetMonthlyRevenuesByTenantQuery : IRequest<List<MonthlyRevenueDto>>
    {
        public Guid TenantId { get; set; }
    }
}
