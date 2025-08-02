using Iyaspark.Application.DTOs.Contract;
using MediatR;
using System.Collections.Generic;

namespace Iyaspark.Application.Modules.Contracts.Queries
{
    public class GetAllContractsQuery : IRequest<List<ContractDto>>
    {
    }
}
