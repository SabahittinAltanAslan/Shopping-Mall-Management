using MediatR;

namespace Iyaspark.Application.Modules.MonthlyRevenue.Commands
{
    public class CreateMonthlyRevenueCommand : IRequest<Unit>
    {
        public CreateMonthlyRevenueDto Dto { get; set; }

        public CreateMonthlyRevenueCommand(CreateMonthlyRevenueDto dto)
        {
            Dto = dto;
        }
    }
}
