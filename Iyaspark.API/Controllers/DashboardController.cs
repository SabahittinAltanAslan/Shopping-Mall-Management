using Iyaspark.Application.Modules.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iyaspark.API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("floor-profit")]
        public async Task<IActionResult> GetFloorProfit()
        {
            var result = await _mediator.Send(new GetFloorProfitQuery());
            return Ok(result);
        }
        [HttpGet("facade-profit-trend")]
        public async Task<IActionResult> GetFacadeProfitTrend()
        {
            var result = await _mediator.Send(new GetFacadeProfitTrendQuery());
            return Ok(result);
        }
        [HttpGet("sector-comparison")]
        public async Task<IActionResult> GetSectorComparison()
        {
            var result = await _mediator.Send(new GetSectorComparisonQuery());
            return Ok(result);
        }
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _mediator.Send(new GetDashboardSummaryQuery());
            return Ok(result);
        }



    }
}
