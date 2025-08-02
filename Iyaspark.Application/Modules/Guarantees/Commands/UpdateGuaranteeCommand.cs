using Iyaspark.Application.DTOs;
using Iyaspark.Application.Modules.Guarantees.DTOs;
using MediatR;
using System;

namespace Iyaspark.Application.Modules.Guarantee.Commands
{
    public class UpdateGuaranteeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public CreateGuaranteeDto GuaranteeDto { get; set; }
    }
}
