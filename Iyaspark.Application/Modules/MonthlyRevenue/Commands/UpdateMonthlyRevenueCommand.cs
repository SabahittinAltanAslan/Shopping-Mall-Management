using Iyaspark.Application.Modules.MonthlyRevenues.DTOs;
using MediatR;

namespace Iyaspark.Application.Modules.MonthlyRevenues.Commands
{
    public class UpdateMonthlyRevenueCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public UpdateMonthlyRevenueDto Dto { get; set; }

        public UpdateMonthlyRevenueCommand(int id, UpdateMonthlyRevenueDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }
}
