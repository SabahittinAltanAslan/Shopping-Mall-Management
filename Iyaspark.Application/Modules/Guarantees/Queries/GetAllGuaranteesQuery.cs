using Iyaspark.Application.Modules.Guarantees.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Iyaspark.Application.Modules.Guarantees.Queries
{
    public class GetAllGuaranteesQuery : IRequest<List<GuaranteeDto>>
    {
    }
}
