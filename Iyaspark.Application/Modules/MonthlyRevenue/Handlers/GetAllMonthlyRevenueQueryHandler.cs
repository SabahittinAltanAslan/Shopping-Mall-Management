using AutoMapper;
using Iyaspark.Application.Modules.MonthlyRevenue.Queries;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.MonthlyRevenue.Handlers
{
    public class GetAllMonthlyRevenueQueryHandler : IRequestHandler<GetAllMonthlyRevenueQuery, List<MonthlyRevenueDto>>
    {
        private readonly IMonthlyRevenueRepository _repository;
        private readonly IMapper _mapper;

        public GetAllMonthlyRevenueQueryHandler(IMonthlyRevenueRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MonthlyRevenueDto>> Handle(GetAllMonthlyRevenueQuery request, CancellationToken cancellationToken)
        {
            var revenues = await _repository.GetAllWithTenantAsync(); // tenant bilgisi dahil
            return _mapper.Map<List<MonthlyRevenueDto>>(revenues);
        }
    }
}
