using Iyaspark.Application.DTOs.Tenant;
using MediatR;
using System.Collections.Generic;

namespace Iyaspark.Application.Modules.Tenant.Queries
{
    public class GetAllTenantsQuery : IRequest<List<TenantDto>>
    {
    }
}
