using AutoMapper;
using Iyaspark.Application.Modules.Contracts.Commands;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Contracts.Handlers
{
    public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand, Guid>
    {
        private readonly IContractRepository _repository;
        private readonly IMapper _mapper;

        public CreateContractCommandHandler(IContractRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateContractCommand request, CancellationToken cancellationToken)
        {
            var contract = _mapper.Map<Domain.Entities.Contract>(request.Dto);
            contract.Id = Guid.NewGuid();

            await _repository.AddAsync(contract);
            await _repository.SaveAsync();

            return contract.Id;
        }
    }
}
