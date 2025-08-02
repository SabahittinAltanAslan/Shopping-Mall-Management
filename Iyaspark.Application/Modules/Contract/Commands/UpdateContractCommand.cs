using Iyaspark.Application.Modules.Contracts.DTOs;
using MediatR;

namespace Iyaspark.Application.Modules.Contracts.Commands
{
    public class UpdateContractCommand : IRequest
    {
        public UpdateContractDto Dto { get; set; }

        public UpdateContractCommand(UpdateContractDto dto)
        {
            Dto = dto;
        }
    }
}
