using Iyaspark.Application.Modules.Reports.DTOs;
using Iyaspark.Application.Modules.Reports.Queries;
using Iyaspark.Application.Modules.Reports.Requests;
using Iyaspark.Domain.Enums;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Reports.Handlers
{
    public class GetProfitabilityReportQueryHandler : IRequestHandler<GetProfitabilityReportQuery, List<ProfitabilityReportDto>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMonthlyRevenueRepository _revenueRepository;

        public GetProfitabilityReportQueryHandler(
            ITenantRepository tenantRepository,
            IContractRepository contractRepository,
            IMonthlyRevenueRepository revenueRepository)
        {
            _tenantRepository = tenantRepository;
            _contractRepository = contractRepository;
            _revenueRepository = revenueRepository;
        }

        public async Task<List<ProfitabilityReportDto>> Handle(GetProfitabilityReportQuery request, CancellationToken cancellationToken)
        {
            var filters = request.Filters;
            var tenants = await _tenantRepository.GetAllAsync();

            // 🔍 Filtreleme
            if (!string.IsNullOrEmpty(filters.Sector))
                tenants = tenants.Where(t => t.Sector.ToString() == filters.Sector).ToList();

            if (!string.IsNullOrEmpty(filters.FacadeDirection))
                tenants = tenants.Where(t => t.FacadeDirection.ToString() == filters.FacadeDirection).ToList();

            if (!string.IsNullOrEmpty(filters.TenantType))
                tenants = tenants.Where(t => t.TenantType.ToString() == filters.TenantType).ToList();

            if (!string.IsNullOrEmpty(filters.FloorLabel))
                tenants = tenants.Where(t => t.FloorLabel.ToString() == filters.FloorLabel).ToList();

            if (filters.HasExtraStorage.HasValue)
                tenants = tenants.Where(t => t.HasExtraStorage == filters.HasExtraStorage.Value).ToList();

            if (filters.SquareMeter.HasValue)
                tenants = tenants.Where(t => t.SquareMeter == filters.SquareMeter.Value).ToList();

            var result = new List<ProfitabilityReportDto>();

            foreach (var tenant in tenants)
            {
                var contract = (await _contractRepository.GetByTenantIdAsync(tenant.Id))
                    .OrderByDescending(c => c.StartDate)
                    .FirstOrDefault();

                if (contract == null)
                    continue;

                var revenues = await _revenueRepository.GetByTenantIdAsync(tenant.Id);
                if (filters.StartDate.HasValue)
                    revenues = revenues.Where(r => new DateTime(r.Year, r.Month, 1) >= filters.StartDate.Value).ToList();

                if (filters.EndDate.HasValue)
                    revenues = revenues.Where(r => new DateTime(r.Year, r.Month, 1) <= filters.EndDate.Value).ToList();

                decimal totalRevenue = 0;
                decimal totalRent = 0;
                decimal totalRevenueBasedRent = 0;


                foreach (var revenue in revenues)
                {
                    totalRevenue += revenue.RevenueAmount;
                    decimal monthlyRent = 0;

                    // Sabit kira her ay eklenir (varsa)
                    if (contract.RentType == RentType.Sabit ||
                        contract.RentType == RentType.SabitVeCiro ||
                        contract.RentType == RentType.Kademeli)
                    {
                        monthlyRent += contract.FixedRentAmount ?? 0;
                    }

                    // Ciro bazlı kira
                    if (contract.RentType == RentType.Ciro ||
                        contract.RentType == RentType.SabitVeCiro ||
                        contract.RentType == RentType.Kademeli)
                    {
                        decimal percentage = 0;

                        if (contract.RentType == RentType.Kademeli)
                        {
                            percentage = revenue.RevenueAmount >= (contract.RevenueTarget ?? 0)
                                ? (decimal)(contract.AboveTargetPercentage ?? 0)
                                : (decimal)(contract.BelowTargetPercentage ?? 0);
                        }
                        else
                        {
                            percentage = (decimal)(contract.RevenuePercentage ?? 0);
                        }


                        decimal calculatedRevenueRent = revenue.RevenueAmount * percentage / 100;
                        monthlyRent += calculatedRevenueRent;
                        totalRevenueBasedRent += calculatedRevenueRent;

                    }

                    // Ekstra depo kirası eklenir
                    if (contract.ExtraStorageRent.HasValue)
                    {
                        monthlyRent += contract.ExtraStorageRent.Value;
                    }

                    totalRent += monthlyRent;
                }

                var dto = new ProfitabilityReportDto
                {
                    CompanyName = tenant.CompanyName,
                    TaxNumber = tenant.TaxNumber,
                    Sector = tenant.Sector.ToString(),
                    FacadeDirection = tenant.FacadeDirection.ToString(),
                    TenantType = tenant.TenantType.ToString(),
                    FloorCode = tenant.FloorCode,
                    FloorLabel = tenant.FloorLabel,
                    SquareMeter = tenant.SquareMeter,
                    RentType = contract.RentType.ToString(),
                    FixedRentAmount = contract.FixedRentAmount,
                    RevenueBasedRent = totalRevenueBasedRent, // Artık detay hesaplandı; toplam zaten TotalRent'te var
                    ExtraStorageRent = contract.ExtraStorageRent ?? 0,
                    TotalRevenue = totalRevenue,
                    TotalRent = totalRent
                };

                result.Add(dto);
            }

            return result;
        }
    }
}
