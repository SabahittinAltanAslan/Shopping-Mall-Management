using Iyaspark.Application.Modules.Guarantee.Commands;
using Iyaspark.Application.Modules.Guarantees.Commands;
using Iyaspark.Application.Modules.Guarantees.DTOs;
using Iyaspark.Application.Modules.Guarantees.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iyaspark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class GuaranteeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuaranteeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> CreateGuarantee([FromBody] CreateGuaranteeDto dto)
        {
            var command = new CreateGuaranteeCommand(dto);
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGuarantee(Guid id, [FromBody] CreateGuaranteeDto dto)
        {
            var command = new UpdateGuaranteeCommand { Id = id, GuaranteeDto = dto };
            var success = await _mediator.Send(command);
            if (!success)
                return NotFound();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuarantee(Guid id)
        {
            var command = new DeleteGuaranteeCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        // GET ALL
        [HttpGet]
        public async Task<ActionResult<List<GuaranteeDto>>> GetAllGuarantees()
        {
            var query = new GetAllGuaranteesQuery();
            var list = await _mediator.Send(query);
            return Ok(list);
        }

        // GET BY TENANT ID
        [HttpGet("tenant/{tenantId}")]
        public async Task<ActionResult<List<GuaranteeDto>>> GetByTenantId(Guid tenantId)
        {
            var query = new GetGuaranteesByTenantIdQuery(tenantId);
            var list = await _mediator.Send(query);
            return Ok(list);
        }
    }
}
