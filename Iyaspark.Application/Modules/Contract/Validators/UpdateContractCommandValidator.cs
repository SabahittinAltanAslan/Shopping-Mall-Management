using FluentValidation;
using Iyaspark.Application.Modules.Contracts.Commands;
using Iyaspark.Domain.Interfaces;

namespace Iyaspark.Application.Modules.Contracts.Validators
{
    public class UpdateContractCommandValidator : AbstractValidator<UpdateContractCommand>
    {
        private readonly IContractRepository _contractRepository;

        public UpdateContractCommandValidator(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;

            RuleFor(x => x.Dto.StartDate)
                .LessThan(x => x.Dto.EndDate)
                .WithMessage("Başlangıç tarihi, bitiş tarihinden önce olmalıdır.");

            RuleFor(x => x)
                .MustAsync(NotOverlapExistingContract)
                .WithMessage("Bu kiracı için aynı tarih aralığında geçerli başka bir sözleşme mevcut.");
        }

        private async Task<bool> NotOverlapExistingContract(UpdateContractCommand command, CancellationToken token)
        {
            return !await _contractRepository.HasOverlappingContractAsync(
                command.Dto.TenantId,
                command.Dto.StartDate,
                command.Dto.EndDate,
                command.Dto.Id // update olduğu için mevcudu hariç tut
            );
        }
    }
}
