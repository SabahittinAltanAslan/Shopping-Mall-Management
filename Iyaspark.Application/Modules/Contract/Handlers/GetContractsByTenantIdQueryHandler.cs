using AutoMapper;
using Iyaspark.Application.DTOs.Contract;
using Iyaspark.Application.Modules.Contract.Queries;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Contract.Handlers
{
    public class GetContractsByTenantIdQueryHandler : IRequestHandler<GetContractsByTenantIdQuery, List<ContractDto>>
    {
        private readonly IContractRepository _repository;
        private readonly IMapper _mapper;

        public GetContractsByTenantIdQueryHandler(IContractRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ContractDto>> Handle(GetContractsByTenantIdQuery request, CancellationToken cancellationToken)
        {
            var contracts = await _repository.GetByTenantIdAsync(request.TenantId);
            return _mapper.Map<List<ContractDto>>(contracts);
        }
    }
}
