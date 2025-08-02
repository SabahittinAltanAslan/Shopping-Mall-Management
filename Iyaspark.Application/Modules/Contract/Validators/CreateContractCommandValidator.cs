using FluentValidation;
using Iyaspark.Application.Modules.Contracts.Commands;
using Iyaspark.Domain.Interfaces;

namespace Iyaspark.Application.Modules.Contracts.Validators
{
    public class CreateContractCommandValidator : AbstractValidator<CreateContractCommand>
    {
        private readonly IContractRepository _contractRepository;

        public CreateContractCommandValidator(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;

            RuleFor(x => x.Dto.StartDate)
                .LessThan(x => x.Dto.EndDate)
                .WithMessage("Başlangıç tarihi, bitiş tarihinden önce olmalıdır.");

            RuleFor(x => x)
                .MustAsync(NotOverlapExistingContract)
                .WithMessage("Bu kiracı için aynı tarih aralığında geçerli başka bir sözleşme mevcut.");
        }

        private async Task<bool> NotOverlapExistingContract(CreateContractCommand command, CancellationToken token)
        {
            return !await _contractRepository.HasOverlappingContractAsync(
                command.Dto.TenantId,
                command.Dto.StartDate,
                command.Dto.EndDate,
                null 
            );
        }
    }
}
