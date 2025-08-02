using Iyaspark.Application.DTOs.Dashboard;
using MediatR;
using System.Collections.Generic;

namespace Iyaspark.Application.Modules.Dashboard.Queries
{
    public class GetFloorProfitQuery : IRequest<List<FloorProfitDto>>
    {
    }
}
