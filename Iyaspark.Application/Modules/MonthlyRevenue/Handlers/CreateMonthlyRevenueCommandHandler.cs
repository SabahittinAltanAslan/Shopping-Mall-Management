using AutoMapper;
using Iyaspark.Application.Modules.MonthlyRevenue.Commands;
using Iyaspark.Domain.Entities;
using Iyaspark.Domain.Interfaces;
using MediatR;
using Entity = Iyaspark.Domain.Entities.MonthlyRevenue;

namespace Iyaspark.Application.Modules.MonthlyRevenue.Handlers
{
    public class CreateMonthlyRevenueCommandHandler : IRequestHandler<CreateMonthlyRevenueCommand, Unit>
    {
        private readonly IMonthlyRevenueRepository _repository;
        private readonly IMapper _mapper;

        public CreateMonthlyRevenueCommandHandler(IMonthlyRevenueRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateMonthlyRevenueCommand request, CancellationToken cancellationToken)
        {
            // 👇 Aynı ayda ciro var mı? kontrolü burada
            bool exists = await _repository.ExistsAsync(
                tenantId: request.Dto.TenantId,
                year: request.Dto.Year,
                month: request.Dto.Month,
                excludeId: null
            );

            if (exists)
                throw new Exception("Bu kiracı için aynı ayda zaten bir ciro kaydı var.");

            var entity = _mapper.Map<Entity>(request.Dto);
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
