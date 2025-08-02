using Iyaspark.Application.Interfaces.Services;
using Iyaspark.Application.Modules.Rents.Queries;
using Iyaspark.Application.Modules.Reports.DTOs;
using Iyaspark.Application.Modules.Reports.Queries;
using Iyaspark.Application.Modules.Reports.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iyaspark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IExcelExportService _excelExportService;

        public ReportController(IMediator mediator, IExcelExportService excelExportService)
        {
            _mediator = mediator;
            _excelExportService = excelExportService;
        }

        /// 📊 Karlılık raporunu JSON formatında döndürür (listeleme ekranı için)
        [HttpPost("profitability")]
        public async Task<ActionResult<List<ProfitabilityReportDto>>> GetProfitabilityReport([FromBody] ReportFilterRequest request)
        {
            var result = await _mediator.Send(new GetProfitabilityReportQuery(request));
            return Ok(result);
        }

        /// 📥 Karlılık raporunu Excel dosyası olarak indirir
        [HttpPost("profitability/excel")]
        public async Task<IActionResult> ExportProfitabilityReportToExcel([FromBody] ReportFilterRequest request)
        {
            var data = await _mediator.Send(new GetProfitabilityReportQuery(request));
            var excelBytes = _excelExportService.GenerateProfitabilityReportExcel(data);

            return File(excelBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"KarlilikRaporu_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }

        // 🔹 Ciro Raporları
        [HttpPost("revenue")]
        public async Task<ActionResult<List<RevenueReportDto>>> GetRevenueReport([FromBody] ReportFilterRequest request)
        {
            var result = await _mediator.Send(new GetRevenueReportQuery(request));
            return Ok(result);
        }

        [HttpPost("revenue/excel")]
        public async Task<IActionResult> ExportRevenueReportToExcel([FromBody] ReportFilterRequest request)
        {
            var data = await _mediator.Send(new GetRevenueReportQuery(request));
            var excelBytes = _excelExportService.GenerateRevenueReportExcel(data);
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"CiroRaporu_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }


        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyRents([FromQuery] int year, [FromQuery] int month)
        {
            var query = new GetMonthlyRentsQuery(year, month);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
