using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Contracts.Commands
{
    public class DeleteContractCommandHandler : IRequestHandler<DeleteContractCommand>
    {
        private readonly IContractRepository _contractRepository;

        public DeleteContractCommandHandler(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task<Unit> Handle(DeleteContractCommand request, CancellationToken cancellationToken)
        {
            await _contractRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
