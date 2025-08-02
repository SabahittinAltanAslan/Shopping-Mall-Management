using Iyaspark.Application.DTOs.Contract;
using MediatR;

namespace Iyaspark.Application.Modules.Contracts.Commands
{
    public class CreateContractCommand : IRequest<Guid>
    {
        public CreateContractDto Dto { get; set; }

        public CreateContractCommand(CreateContractDto dto)
        {
            Dto = dto;
        }
    }
}
