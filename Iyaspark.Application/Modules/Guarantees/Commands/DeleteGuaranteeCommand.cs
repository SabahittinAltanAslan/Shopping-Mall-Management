using MediatR;
using System;

namespace Iyaspark.Application.Modules.Guarantees.Commands
{
    public class DeleteGuaranteeCommand : IRequest
    {
        public Guid Id { get; set; }

        public DeleteGuaranteeCommand(Guid id)
        {
            Id = id;
        }
    }
}
