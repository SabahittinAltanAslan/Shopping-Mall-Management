using Iyaspark.Application.Modules.MonthlyRevenue.Commands;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.MonthlyRevenue.Handlers
{
    public class DeleteMonthlyRevenueCommandHandler : IRequestHandler<DeleteMonthlyRevenueCommand>
    {
        private readonly IMonthlyRevenueRepository _repository;

        public DeleteMonthlyRevenueCommandHandler(IMonthlyRevenueRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteMonthlyRevenueCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id);
            await _repository.SaveAsync();
            return Unit.Value;
        }
    }
}
