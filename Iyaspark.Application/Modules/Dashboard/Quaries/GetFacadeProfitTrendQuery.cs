using Iyaspark.Application.Modules.Dashboard.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Iyaspark.Application.Modules.Dashboard.Queries
{
    public class GetFacadeProfitTrendQuery : IRequest<List<FacadeProfitTrendDto>>
    {
    }
}
