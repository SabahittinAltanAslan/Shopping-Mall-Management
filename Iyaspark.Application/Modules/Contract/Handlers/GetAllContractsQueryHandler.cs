using AutoMapper;
using Iyaspark.Application.DTOs.Contract;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Contracts.Queries
{
    public class GetAllContractsQueryHandler : IRequestHandler<GetAllContractsQuery, List<ContractDto>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IMapper _mapper;

        public GetAllContractsQueryHandler(IContractRepository contractRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _mapper = mapper;
        }

        public async Task<List<ContractDto>> Handle(GetAllContractsQuery request, CancellationToken cancellationToken)
        {
            var contracts = await _contractRepository.GetAllAsync();
            return _mapper.Map<List<ContractDto>>(contracts);
        }
    }
}
