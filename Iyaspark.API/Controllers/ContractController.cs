using Iyaspark.Application.DTOs.Contract;
using Iyaspark.Application.Modules.Contract.Queries;
using Iyaspark.Application.Modules.Contracts.Commands;
using Iyaspark.Application.Modules.Contracts.DTOs;
using Iyaspark.Application.Modules.Contracts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iyaspark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ContractController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContractController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ✅ Tüm sözleşmeleri getir
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllContractsQuery());
            return Ok(result);
        }

        // ✅ Kiracıya ait sözleşmeleri getir
        [HttpGet("by-tenant/{tenantId}")]
        public async Task<IActionResult> GetByTenant(Guid tenantId)
        {
            var query = new GetContractsByTenantIdQuery(tenantId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // ✅ Yeni sözleşme ekle (PDF içerebilir)
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateContractDto dto)
        {
            var command = new CreateContractCommand(dto);
            var newId = await _mediator.Send(command);
            return Ok(new { id = newId });
        }

        // ✅ Sözleşme güncelle (PDF içerebilir)
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateContractDto dto)
        {
            var command = new UpdateContractCommand(dto);
            await _mediator.Send(command);
            return NoContent();
        }

        // ✅ Sözleşme sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteContractCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        // ✅ PDF upload (internal only - optional, for fallback)
        [HttpPost("upload")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadPdf([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya bulunamadı.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "contracts");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"contracts/{fileName}";
            return Ok(new { filePath = relativePath });
        }
    }
}
