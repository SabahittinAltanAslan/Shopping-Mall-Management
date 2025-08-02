using MediatR;

namespace Iyaspark.Application.Modules.MonthlyRevenue.Queries
{
    public class GetAllMonthlyRevenueQuery : IRequest<List<MonthlyRevenueDto>>
    {
    }
}
