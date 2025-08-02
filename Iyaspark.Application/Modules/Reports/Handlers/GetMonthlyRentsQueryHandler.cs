using Iyaspark.Application.Modules.Rents.Queries;
using Iyaspark.Application.Modules.Reports.DTOs;
using Iyaspark.Application.Modules.Reports.Queries;
using Iyaspark.Domain.Enums;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Reports.Handlers
{
    public class GetMonthlyRentsQueryHandler : IRequestHandler<GetMonthlyRentsQuery, List<MonthlyRentDto>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMonthlyRevenueRepository _revenueRepository;

        public GetMonthlyRentsQueryHandler(
            ITenantRepository tenantRepository,
            IContractRepository contractRepository,
            IMonthlyRevenueRepository revenueRepository)
        {
            _tenantRepository = tenantRepository;
            _contractRepository = contractRepository;
            _revenueRepository = revenueRepository;
        }

        public async Task<List<MonthlyRentDto>> Handle(GetMonthlyRentsQuery request, CancellationToken cancellationToken)
        {
            var tenants = await _tenantRepository.GetAllAsync();
            var contracts = await _contractRepository.GetAllWithTenantAsync();
            var revenues = await _revenueRepository.GetAllAsync();

            var result = new List<MonthlyRentDto>();
            var targetDate = new DateTime(request.Year, request.Month, 1);

            foreach (var tenant in tenants)
            {
                var contract = contracts.FirstOrDefault(c =>
                    c.TenantId == tenant.Id &&
                    c.StartDate <= targetDate &&
                    c.EndDate >= targetDate);

                if (contract == null)
                    continue;

                decimal rentAmount = 0;

                switch (contract.RentType)
                {
                    case RentType.Sabit:
                        rentAmount = contract.FixedRentAmount ?? 0;
                        break;

                    case RentType.Ciro:
                        var revenue1 = revenues.FirstOrDefault(r =>
                            r.TenantId == tenant.Id &&
                            r.Month == request.Month &&
                            r.Year == request.Year);

                        if (revenue1 == null || !contract.RevenuePercentage.HasValue)
                            continue;

                        rentAmount = revenue1.RevenueAmount * ((decimal)contract.RevenuePercentage.Value / 100m);
                        break;

                    case RentType.SabitVeCiro:
                        var revenue2 = revenues.FirstOrDefault(r =>
                            r.TenantId == tenant.Id &&
                            r.Month == request.Month &&
                            r.Year == request.Year);

                        if (revenue2 == null || !contract.RevenuePercentage.HasValue)
                            continue;

                        rentAmount = (contract.FixedRentAmount ?? 0) +
                                     (revenue2.RevenueAmount * ((decimal)contract.RevenuePercentage.Value / 100m));
                        break;

                    case RentType.Kademeli:
                        var revenue3 = revenues.FirstOrDefault(r =>
                            r.TenantId == tenant.Id &&
                            r.Month == request.Month &&
                            r.Year == request.Year);

                        if (revenue3 == null || !contract.RevenueTarget.HasValue ||
                            !contract.AboveTargetPercentage.HasValue || !contract.BelowTargetPercentage.HasValue)
                            continue;

                        if (revenue3.RevenueAmount >= contract.RevenueTarget.Value)
                        {
                            rentAmount = revenue3.RevenueAmount * ((decimal)contract.AboveTargetPercentage.Value / 100m);
                        }
                        else
                        {
                            rentAmount = revenue3.RevenueAmount * ((decimal)contract.BelowTargetPercentage.Value / 100m);
                        }
                        break;
                }


                
                    rentAmount += contract.ExtraStorageRent ?? 0;


                result.Add(new MonthlyRentDto
                {
                    CompanyName = tenant.CompanyName,
                    TaxNumber = tenant.TaxNumber,
                    FloorLabel = tenant.FloorLabel,
                    FacadeDirection = tenant.FacadeDirection.ToString(),
                    RentType = contract.RentType.ToString(),
                    Month = request.Month,
                    Year = request.Year,
                    RentAmount = Math.Round(rentAmount, 2)
                });
            }

            return result;
        }
    }
}
