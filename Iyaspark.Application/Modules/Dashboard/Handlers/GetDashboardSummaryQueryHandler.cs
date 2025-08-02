using Iyaspark.Application.Modules.Dashboard.DTOs;
using Iyaspark.Application.Modules.Dashboard.Queries;
using Iyaspark.Domain.Enums;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Dashboard.Handlers
{
    public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMonthlyRevenueRepository _revenueRepository;

        public GetDashboardSummaryQueryHandler(
            ITenantRepository tenantRepository,
            IContractRepository contractRepository,
            IMonthlyRevenueRepository revenueRepository)
        {
            _tenantRepository = tenantRepository;
            _contractRepository = contractRepository;
            _revenueRepository = revenueRepository;
        }

        public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            var tenants = await _tenantRepository.GetAllAsync();

            var floorProfits = new Dictionary<string, decimal>();
            var facadeProfits = new Dictionary<string, decimal>();
            var tenantProfits = new Dictionary<string, decimal>();

            decimal totalProfit = 0;

            foreach (var tenant in tenants)
            {
                var contract = (await _contractRepository.GetByTenantIdAsync(tenant.Id))
                    .OrderByDescending(c => c.StartDate)
                    .FirstOrDefault();
                if (contract == null) continue;

                var revenues = await _revenueRepository.GetByTenantIdAsync(tenant.Id);
                decimal tenantProfit = 0;

                foreach (var revenue in revenues)
                {
                    decimal monthlyRent = 0;

                    if (contract.RentType == RentType.Sabit ||
                        contract.RentType == RentType.SabitVeCiro ||
                        contract.RentType == RentType.Kademeli)
                        monthlyRent += contract.FixedRentAmount ?? 0;

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

                        monthlyRent += revenue.RevenueAmount * percentage / 100;
                    }

                    if (contract.ExtraStorageRent.HasValue)
                        monthlyRent += contract.ExtraStorageRent.Value;

                    tenantProfit += monthlyRent;
                }

                // Toplam kâr
                totalProfit += tenantProfit;

                // Floor kârı ekle
                if (!string.IsNullOrEmpty(tenant.FloorLabel))
                {
                    if (!floorProfits.ContainsKey(tenant.FloorLabel))
                        floorProfits[tenant.FloorLabel] = 0;
                    floorProfits[tenant.FloorLabel] += tenantProfit;
                }

                // Cephe kârı ekle
                var facadeKey = tenant.FacadeDirection.ToString();
                if (!facadeProfits.ContainsKey(facadeKey))
                    facadeProfits[facadeKey] = 0;
                facadeProfits[facadeKey] += tenantProfit;

                // Tenant bazlı ekle
                if (!tenantProfits.ContainsKey(tenant.CompanyName))
                    tenantProfits[tenant.CompanyName] = 0;
                tenantProfits[tenant.CompanyName] += tenantProfit;
            }

            return new DashboardSummaryDto
            {
                TotalProfit = totalProfit,
                TopFloor = floorProfits.OrderByDescending(x => x.Value).FirstOrDefault().Key ?? "-",
                TopFacade = facadeProfits.OrderByDescending(x => x.Value).FirstOrDefault().Key ?? "-",
                TopTenant = tenantProfits.OrderByDescending(x => x.Value).FirstOrDefault().Key ?? "-"
            };
        }
    }
}
