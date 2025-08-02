using Iyaspark.Application.Modules.Tenant.Commands;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Tenant.Handlers
{
    public class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand>
    {
        private readonly ITenantRepository _repository;

        public DeleteTenantCommandHandler(ITenantRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
