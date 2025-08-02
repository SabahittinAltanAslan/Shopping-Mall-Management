using Iyaspark.Application.Modules.Guarantees.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace Iyaspark.Application.Modules.Guarantees.Queries
{
    public class GetGuaranteesByTenantIdQuery : IRequest<List<GuaranteeDto>>
    {
        public Guid TenantId { get; set; }

        public GetGuaranteesByTenantIdQuery(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }
}
