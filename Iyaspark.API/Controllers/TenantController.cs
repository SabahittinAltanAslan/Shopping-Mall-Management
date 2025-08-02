using Iyaspark.Application.DTOs.Tenant;
using Iyaspark.Application.Modules.Tenant.Commands;
using Iyaspark.Application.Modules.Tenant.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iyaspark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // JWT koruması altında
    public class TenantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Kiracı Listele
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTenantsQuery());
            return Ok(result);
        }

        // Kiracı Ekle
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTenantDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateTenantCommand(dto);
            var newId = await _mediator.Send(command);
            return Ok(new { id = newId });
        }

        // Güncelle
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] CreateTenantDto dto)
        {
            var command = new UpdateTenantCommand(id, dto);
            await _mediator.Send(command);
            return Ok();
        }

        // Sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteTenantCommand(id));
            return Ok();
        }

    }
}
