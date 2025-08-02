using Iyaspark.Application.Modules.MonthlyRevenues.Commands;
using Iyaspark.Domain.Entities;
using Iyaspark.Domain.Enums;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.MonthlyRevenues.Handlers
{
    public class UpdateMonthlyRevenueCommandHandler : IRequestHandler<UpdateMonthlyRevenueCommand, Unit>
    {
        private readonly IMonthlyRevenueRepository _repository;

        public UpdateMonthlyRevenueCommandHandler(IMonthlyRevenueRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateMonthlyRevenueCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing == null)
                throw new Exception("Ciro kaydı bulunamadı.");

            var isDuplicate = await _repository.AnyAsync(x =>
                x.TenantId == request.Dto.TenantId &&
                x.Year == request.Dto.Year &&
                x.Month == request.Dto.Month &&
                x.Id != request.Id);

            if (isDuplicate)
                throw new Exception("Aynı kiracı için aynı yıl ve ayda başka bir ciro kaydı mevcut.");

            existing.TenantId = request.Dto.TenantId;
            existing.Year = request.Dto.Year;
            existing.Month = request.Dto.Month;
            existing.RevenueAmount = (decimal)request.Dto.RevenueAmount;
            existing.CurrencyType = (CurrencyType)request.Dto.CurrencyType;

            try
            {
                await _repository.UpdateAsync(existing);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Kaydederken hata oluştu: " + ex.InnerException?.Message ?? ex.Message);
            }

            return Unit.Value;
        }
    }
}
