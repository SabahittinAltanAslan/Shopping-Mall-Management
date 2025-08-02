using AutoMapper;
using Iyaspark.Application.Modules.Guarantee.Commands;
using Iyaspark.Domain.Entities;
using Iyaspark.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Iyaspark.Application.Modules.Guarantee.Handlers
{
    public class UpdateGuaranteeCommandHandler : IRequestHandler<UpdateGuaranteeCommand, bool>
    {
        private readonly IGuaranteeRepository _repository;
        private readonly IMapper _mapper;

        public UpdateGuaranteeCommandHandler(IGuaranteeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateGuaranteeCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing == null) return false;

            _mapper.Map(request.GuaranteeDto, existing);
            await _repository.UpdateAsync(existing);
            return true;
        }
    }
}
