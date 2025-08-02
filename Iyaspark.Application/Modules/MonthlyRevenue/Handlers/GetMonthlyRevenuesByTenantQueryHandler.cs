using AutoMapper;
using Iyaspark.Application.DTOs;
using Iyaspark.Application.Modules.MonthlyRevenue.Queries;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.MonthlyRevenue.Handlers
{
    public class GetMonthlyRevenuesByTenantQueryHandler : IRequestHandler<GetMonthlyRevenuesByTenantQuery, List<MonthlyRevenueDto>>
    {
        private readonly IMonthlyRevenueRepository _repository;
        private readonly IMapper _mapper;

        public GetMonthlyRevenuesByTenantQueryHandler(IMonthlyRevenueRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MonthlyRevenueDto>> Handle(GetMonthlyRevenuesByTenantQuery request, CancellationToken cancellationToken)
        {
            var revenues = await _repository.GetByTenantIdAsync(request.TenantId);
            return _mapper.Map<List<MonthlyRevenueDto>>(revenues);
        }
    }
}
