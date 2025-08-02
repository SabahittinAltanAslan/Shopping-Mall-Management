using AutoMapper;
using Iyaspark.Application.Modules.Guarantees.DTOs;
using Iyaspark.Application.Modules.Guarantees.Queries;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Guarantees.Handlers
{
    public class GetGuaranteesByTenantIdQueryHandler : IRequestHandler<GetGuaranteesByTenantIdQuery, List<GuaranteeDto>>
    {
        private readonly IGuaranteeRepository _guaranteeRepository;
        private readonly IMapper _mapper;

        public GetGuaranteesByTenantIdQueryHandler(IGuaranteeRepository guaranteeRepository, IMapper mapper)
        {
            _guaranteeRepository = guaranteeRepository;
            _mapper = mapper;
        }

        public async Task<List<GuaranteeDto>> Handle(GetGuaranteesByTenantIdQuery request, CancellationToken cancellationToken)
        {
            var guarantees = await _guaranteeRepository.GetByIdAsync(request.TenantId);
            return _mapper.Map<List<GuaranteeDto>>(guarantees);
        }
    }
}
