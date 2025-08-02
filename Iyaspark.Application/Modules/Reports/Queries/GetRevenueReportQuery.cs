using Iyaspark.Application.Modules.Reports.DTOs;
using Iyaspark.Application.Modules.Reports.Requests;
using MediatR;
using System.Collections.Generic;

namespace Iyaspark.Application.Modules.Reports.Queries
{
    public class GetRevenueReportQuery : IRequest<List<RevenueReportDto>>
    {
        public ReportFilterRequest Filters { get; set; }

        public GetRevenueReportQuery(ReportFilterRequest filters)
        {
            Filters = filters;
        }
    }
}
