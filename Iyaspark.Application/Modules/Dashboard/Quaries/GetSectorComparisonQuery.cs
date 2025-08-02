using Iyaspark.Application.Modules.Dashboard.DTOs;
using MediatR;

namespace Iyaspark.Application.Modules.Dashboard.Queries
{
    public class GetSectorComparisonQuery : IRequest<List<SectorProfitComparisonDto>>
    {
    }
}
