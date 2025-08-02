using Iyaspark.Application.DTOs.Contract;
using MediatR;

namespace Iyaspark.Application.Modules.Contract.Queries
{
    public class GetContractsByTenantIdQuery : IRequest<List<ContractDto>>
    {
        public Guid TenantId { get; }

        public GetContractsByTenantIdQuery(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }
}
