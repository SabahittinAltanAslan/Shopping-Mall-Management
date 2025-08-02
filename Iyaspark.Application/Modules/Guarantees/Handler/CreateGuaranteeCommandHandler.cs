using AutoMapper;
using Iyaspark.Application.Modules.Guarantees.Commands;
using Iyaspark.Application.Modules.Guarantees.DTOs;
using Iyaspark.Domain.Entities;
using Iyaspark.Domain.Interfaces;
using MediatR;
using GuaranteeEntity = Iyaspark.Domain.Entities.Guarantee;


namespace Iyaspark.Application.Modules.Guarantees.Handlers
{
    public class CreateGuaranteeCommandHandler : IRequestHandler<CreateGuaranteeCommand, Guid>
    {
        private readonly IGuaranteeRepository _guaranteeRepository;
        private readonly IMapper _mapper;

        public CreateGuaranteeCommandHandler(IGuaranteeRepository guaranteeRepository, IMapper mapper)
        {
            _guaranteeRepository = guaranteeRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateGuaranteeCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<GuaranteeEntity>(request.GuaranteeDto);
            await _guaranteeRepository.AddAsync(entity);
            return entity.Id;
        }
    }
}
