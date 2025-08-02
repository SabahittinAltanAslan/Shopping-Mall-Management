using Iyaspark.Application.Modules.Guarantees.DTOs;
using MediatR;

namespace Iyaspark.Application.Modules.Guarantees.Commands
{
    public class CreateGuaranteeCommand : IRequest<Guid>
    {
        public CreateGuaranteeDto GuaranteeDto { get; set; }

        public CreateGuaranteeCommand(CreateGuaranteeDto guaranteeDto)
        {
            GuaranteeDto = guaranteeDto;
        }
    }
}
