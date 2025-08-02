using Iyaspark.Application.Modules.Dashboard.DTOs;
using Iyaspark.Application.Modules.Dashboard.Queries;
using Iyaspark.Domain.Enums;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Dashboard.Handlers
{
    public class GetFacadeProfitTrendQueryHandler : IRequestHandler<GetFacadeProfitTrendQuery, List<FacadeProfitTrendDto>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMonthlyRevenueRepository _revenueRepository;

        public GetFacadeProfitTrendQueryHandler(
            ITenantRepository tenantRepository,
            IContractRepository contractRepository,
            IMonthlyRevenueRepository revenueRepository)
        {
            _tenantRepository = tenantRepository;
            _contractRepository = contractRepository;
            _revenueRepository = revenueRepository;
        }

        public async Task<List<FacadeProfitTrendDto>> Handle(GetFacadeProfitTrendQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var thisMonthStart = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = thisMonthStart.AddMonths(-1);

            var tenants = await _tenantRepository.GetAllAsync();
            var grouped = tenants.GroupBy(t => t.FacadeDirection);

            var result = new List<FacadeProfitTrendDto>();

            foreach (var group in grouped)
            {
                decimal thisMonthProfit = 0;
                decimal lastMonthProfit = 0;

                foreach (var tenant in group)
                {
                    var contract = (await _contractRepository.GetByTenantIdAsync(tenant.Id))
                        .OrderByDescending(c => c.StartDate)
                        .FirstOrDefault();
                    if (contract == null) continue;

                    var revenues = await _revenueRepository.GetByTenantIdAsync(tenant.Id);

                    foreach (var revenue in revenues)
                    {
                        var revenueDate = new DateTime(revenue.Year, revenue.Month, 1);
                        if (revenueDate == thisMonthStart || revenueDate == lastMonthStart)
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

                            if (revenueDate == thisMonthStart)
                                thisMonthProfit += monthlyRent;
                            else
                                lastMonthProfit += monthlyRent;
                        }
                    }
                }

                decimal change = lastMonthProfit == 0
                    ? (thisMonthProfit > 0 ? 100 : 0)
                    : ((thisMonthProfit - lastMonthProfit) / lastMonthProfit) * 100;

                result.Add(new FacadeProfitTrendDto
                {
                    FacadeDirection = group.Key.ToString(),
                    ThisMonthProfit = thisMonthProfit,
                    LastMonthProfit = lastMonthProfit,
                    ChangePercentage = Math.Round(change, 2)
                });
            }

            return result;
        }
    }
}
