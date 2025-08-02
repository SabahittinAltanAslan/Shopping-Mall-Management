using AutoMapper;
using Iyaspark.Application.Modules.Contracts.Commands;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Contracts.Handlers
{
    public class UpdateContractCommandHandler : IRequestHandler<UpdateContractCommand>
    {
        private readonly IContractRepository _repository;
        private readonly IMapper _mapper;

        public UpdateContractCommandHandler(IContractRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Dto.Id);
            if (existing == null)
                throw new Exception("Sözleşme bulunamadı");

            _mapper.Map(request.Dto, existing);

            await _repository.SaveAsync();
            return Unit.Value;
        }
    }
}
