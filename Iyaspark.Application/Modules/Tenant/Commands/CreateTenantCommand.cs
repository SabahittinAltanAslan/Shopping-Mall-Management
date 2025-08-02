using Iyaspark.Application.DTOs.Tenant;
using MediatR;

namespace Iyaspark.Application.Modules.Tenant.Commands
{
    public class CreateTenantCommand : IRequest<Guid>
    {
        public CreateTenantDto TenantDto { get; set; }

        public CreateTenantCommand(CreateTenantDto dto)
        {
            TenantDto = dto;
        }
    }
}
