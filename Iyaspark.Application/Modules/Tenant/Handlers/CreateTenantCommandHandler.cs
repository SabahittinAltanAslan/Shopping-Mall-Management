using AutoMapper;
using Iyaspark.Application.Modules.Tenant.Commands;
using Iyaspark.Domain.Entities;
using Iyaspark.Domain.Interfaces;
using MediatR;
using TenantEntity = Iyaspark.Domain.Entities.Tenant;

namespace Iyaspark.Application.Modules.Tenant.Handlers
{
    public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Guid>
    {
        private readonly ITenantRepository _repository;
        private readonly IMapper _mapper;

        public CreateTenantCommandHandler(ITenantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = _mapper.Map<TenantEntity>(request.TenantDto);
            await _repository.AddAsync(tenant);
            return tenant.Id;
        }
    }
}
