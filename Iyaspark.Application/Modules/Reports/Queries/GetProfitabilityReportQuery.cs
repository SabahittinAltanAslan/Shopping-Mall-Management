using Iyaspark.Application.Modules.Reports.DTOs;
using Iyaspark.Application.Modules.Reports.Requests;
using MediatR;
using System.Collections.Generic;

namespace Iyaspark.Application.Modules.Reports.Queries
{
    public class GetProfitabilityReportQuery : IRequest<List<ProfitabilityReportDto>>
    {
        public ReportFilterRequest Filters { get; set; }

        public GetProfitabilityReportQuery(ReportFilterRequest filters)
        {
            Filters = filters;
        }
    }
}
