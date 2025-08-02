using Iyaspark.Application.DTOs.Tenant;
using MediatR;

namespace Iyaspark.Application.Modules.Tenant.Commands
{
    public class UpdateTenantCommand : IRequest
    {
        public Guid Id { get; set; }
        public CreateTenantDto TenantDto { get; set; }

        public UpdateTenantCommand(Guid id, CreateTenantDto dto)
        {
            Id = id;
            TenantDto = dto;
        }
    }
}
