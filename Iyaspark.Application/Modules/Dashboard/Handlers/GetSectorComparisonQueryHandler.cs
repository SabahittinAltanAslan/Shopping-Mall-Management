using Iyaspark.Application.Modules.Dashboard.DTOs;
using Iyaspark.Application.Modules.Dashboard.Queries;
using Iyaspark.Domain.Enums;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Dashboard.Handlers
{
    public class GetSectorComparisonQueryHandler : IRequestHandler<GetSectorComparisonQuery, List<SectorProfitComparisonDto>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMonthlyRevenueRepository _revenueRepository;

        public GetSectorComparisonQueryHandler(
            ITenantRepository tenantRepository,
            IContractRepository contractRepository,
            IMonthlyRevenueRepository revenueRepository)
        {
            _tenantRepository = tenantRepository;
            _contractRepository = contractRepository;
            _revenueRepository = revenueRepository;
        }

        public async Task<List<SectorProfitComparisonDto>> Handle(GetSectorComparisonQuery request, CancellationToken cancellationToken)
        {
            var tenants = await _tenantRepository.GetAllAsync();
            var grouped = tenants.GroupBy(t => t.Sector);

            var result = new List<SectorProfitComparisonDto>();

            foreach (var group in grouped)
            {
                decimal totalRevenue = 0;
                decimal totalRent = 0;

                foreach (var tenant in group)
                {
                    var contract = (await _contractRepository.GetByTenantIdAsync(tenant.Id))
                        .OrderByDescending(c => c.StartDate)
                        .FirstOrDefault();
                    if (contract == null) continue;

                    var revenues = await _revenueRepository.GetByTenantIdAsync(tenant.Id);
                    foreach (var revenue in revenues)
                    {
                        totalRevenue += revenue.RevenueAmount;

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

                        totalRent += monthlyRent;
                    }
                }

                result.Add(new SectorProfitComparisonDto
                {
                    Sector = group.Key.ToString(),
                    TotalRevenue = totalRevenue,
                    TotalRent = totalRent
                });
            }

            return result;
        }
    }
}
