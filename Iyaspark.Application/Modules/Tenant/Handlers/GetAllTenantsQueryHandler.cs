using AutoMapper;
using Iyaspark.Application.DTOs.Tenant;
using Iyaspark.Application.Modules.Tenant.Queries;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Tenant.Handlers
{
    public class GetAllTenantsQueryHandler : IRequestHandler<GetAllTenantsQuery, List<TenantDto>>
    {
        private readonly ITenantRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTenantsQueryHandler(ITenantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TenantDto>> Handle(GetAllTenantsQuery request, CancellationToken cancellationToken)
        {
            var tenants = await _repository.GetAllAsync();
            return _mapper.Map<List<TenantDto>>(tenants);
        }
    }
}
