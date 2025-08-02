using Iyaspark.Application.Modules.Guarantees.Commands;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Guarantees.Handlers
{
    public class DeleteGuaranteeCommandHandler : IRequestHandler<DeleteGuaranteeCommand>
    {
        private readonly IGuaranteeRepository _guaranteeRepository;

        public DeleteGuaranteeCommandHandler(IGuaranteeRepository guaranteeRepository)
        {
            _guaranteeRepository = guaranteeRepository;
        }

        public async Task<Unit> Handle(DeleteGuaranteeCommand request, CancellationToken cancellationToken)
        {
            await _guaranteeRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
