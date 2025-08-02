using AutoMapper;
using Iyaspark.Application.Modules.Guarantees.DTOs;
using Iyaspark.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iyaspark.Application.Modules.Guarantees.Queries
{
    public class GetAllGuaranteesQueryHandler : IRequestHandler<GetAllGuaranteesQuery, List<GuaranteeDto>>
    {
        private readonly IGuaranteeRepository _guaranteeRepository;
        private readonly IMapper _mapper;

        public GetAllGuaranteesQueryHandler(IGuaranteeRepository guaranteeRepository, IMapper mapper)
        {
            _guaranteeRepository = guaranteeRepository;
            _mapper = mapper;
        }

        public async Task<List<GuaranteeDto>> Handle(GetAllGuaranteesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _guaranteeRepository.GetAllWithTenantAsync();
            return _mapper.Map<List<GuaranteeDto>>(entities);
        }
    }
}
