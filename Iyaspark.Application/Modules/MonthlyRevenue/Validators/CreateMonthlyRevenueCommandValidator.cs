using FluentValidation;
using Iyaspark.Application.Modules.MonthlyRevenue.Commands;

namespace Iyaspark.Application.Modules.MonthlyRevenues.Validators
{
    public class CreateMonthlyRevenueCommandValidator : AbstractValidator<CreateMonthlyRevenueCommand>
    {
        public CreateMonthlyRevenueCommandValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Ciro bilgileri boş olamaz.");
            RuleFor(x => x.Dto.RevenueAmount).GreaterThan(0).WithMessage("Ciro tutarı 0'dan büyük olmalıdır.");
            RuleFor(x => x.Dto.Month).InclusiveBetween(1, 12).WithMessage("Ay 1-12 aralığında olmalıdır.");
            RuleFor(x => x.Dto.Year).InclusiveBetween(2000, 2100).WithMessage("Geçerli bir yıl giriniz.");
        }
    }
}
