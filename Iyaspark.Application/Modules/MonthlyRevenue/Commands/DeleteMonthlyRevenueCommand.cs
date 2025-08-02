using MediatR;

namespace Iyaspark.Application.Modules.MonthlyRevenue.Commands
{
    public class DeleteMonthlyRevenueCommand : IRequest
    {
        public int Id { get; set; }
    }
}
