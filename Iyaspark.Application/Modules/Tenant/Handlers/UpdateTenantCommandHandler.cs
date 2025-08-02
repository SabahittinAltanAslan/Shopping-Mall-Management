using AutoMapper;
using Iyaspark.Application.Modules.Tenant.Commands;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Tenant.Handlers
{
    public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand>
    {
        private readonly ITenantRepository _repository;
        private readonly IMapper _mapper;

        public UpdateTenantCommandHandler(ITenantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing == null)
                throw new Exception("Kiracı bulunamadı.");

            _mapper.Map(request.TenantDto, existing);
            await _repository.UpdateAsync(existing);

            return Unit.Value;
        }
    }
}
