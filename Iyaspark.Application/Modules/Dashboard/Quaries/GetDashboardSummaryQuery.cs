using Iyaspark.Application.Modules.Dashboard.DTOs;
using MediatR;

namespace Iyaspark.Application.Modules.Dashboard.Queries
{
    public class GetDashboardSummaryQuery : IRequest<DashboardSummaryDto>
    {
    }
}
