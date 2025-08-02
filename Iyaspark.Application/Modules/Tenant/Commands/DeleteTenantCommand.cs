using MediatR;

namespace Iyaspark.Application.Modules.Tenant.Commands
{
    public class DeleteTenantCommand : IRequest
    {
        public Guid Id { get; }

        public DeleteTenantCommand(Guid id)
        {
            Id = id;
        }
    }
}
