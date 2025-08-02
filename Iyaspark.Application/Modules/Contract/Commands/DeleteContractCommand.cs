using MediatR;
using System;

namespace Iyaspark.Application.Modules.Contracts.Commands
{
    public class DeleteContractCommand : IRequest
    {
        public Guid Id { get; }

        public DeleteContractCommand(Guid id)
        {
            Id = id;
        }
    }
}
