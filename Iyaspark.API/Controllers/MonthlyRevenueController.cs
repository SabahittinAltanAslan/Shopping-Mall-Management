using Iyaspark.Application.Modules.MonthlyRevenue.Commands;
using Iyaspark.Application.Modules.MonthlyRevenue.Queries;
using Iyaspark.Application.Modules.MonthlyRevenues.Commands;
using Iyaspark.Application.Modules.MonthlyRevenues.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iyaspark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MonthlyRevenueController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MonthlyRevenueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 🟢 CREATE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMonthlyRevenueCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "Ciro kaydı başarıyla oluşturuldu." });
        }

        // 🟡 UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMonthlyRevenue(int id, [FromBody] UpdateMonthlyRevenueDto dto)
        {
            var command = new UpdateMonthlyRevenueCommand(id, dto);
            await _mediator.Send(command);
            return Ok("Ciro kaydı başarıyla güncellendi.");
        }



        // 🔴 DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteMonthlyRevenueCommand { Id = id });
            return NoContent();
        }


        // 🔵 GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllMonthlyRevenueQuery());
            return Ok(result);
        }

        // 🟣 GET BY TENANT
        [HttpGet("by-tenant/{tenantId}")]
        public async Task<IActionResult> GetByTenantId([FromRoute] Guid tenantId)
        {
            var result = await _mediator.Send(new GetMonthlyRevenuesByTenantQuery { TenantId = tenantId });
            return Ok(result);
        }
    }
}
