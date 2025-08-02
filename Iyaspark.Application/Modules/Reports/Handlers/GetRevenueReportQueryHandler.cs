using Iyaspark.Application.Modules.Reports.DTOs;
using Iyaspark.Application.Modules.Reports.Queries;
using Iyaspark.Application.Modules.Reports.Requests;
using Iyaspark.Domain.Interfaces;
using MediatR;

namespace Iyaspark.Application.Modules.Reports.Handlers
{
    public class GetRevenueReportQueryHandler : IRequestHandler<GetRevenueReportQuery, List<RevenueReportDto>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMonthlyRevenueRepository _revenueRepository;

        public GetRevenueReportQueryHandler(
            ITenantRepository tenantRepository,
            IMonthlyRevenueRepository revenueRepository)
        {
            _tenantRepository = tenantRepository;
            _revenueRepository = revenueRepository;
        }

        public async Task<List<RevenueReportDto>> Handle(GetRevenueReportQuery request, CancellationToken cancellationToken)
        {
            var filters = request.Filters;

            var tenants = await _tenantRepository.GetAllAsync();

            // 🔍 Tenant filtreleri
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

            var result = new List<RevenueReportDto>();

            foreach (var tenant in tenants)
            {
                var revenues = await _revenueRepository.GetByTenantIdAsync(tenant.Id);

                // Tarih filtresi
                if (filters.StartDate.HasValue)
                    revenues = revenues.Where(r =>
                        new DateTime(r.Year, r.Month, 1) >= filters.StartDate.Value).ToList();

                if (filters.EndDate.HasValue)
                    revenues = revenues.Where(r =>
                        new DateTime(r.Year, r.Month, 1) <= filters.EndDate.Value).ToList();

                var totalRevenue = revenues.Sum(r => r.RevenueAmount);

                var dto = new RevenueReportDto
                {
                    CompanyName = tenant.CompanyName,
                    TaxNumber = tenant.TaxNumber,
                    Sector = tenant.Sector.ToString(),
                    FacadeDirection = tenant.FacadeDirection.ToString(),
                    TenantType = tenant.TenantType.ToString(),
                    FloorCode = tenant.FloorCode,
                    FloorLabel = tenant.FloorLabel,
                    SquareMeter = tenant.SquareMeter,
                    TotalRevenue = totalRevenue
                };

                result.Add(dto);
            }

            return result;
        }
    }
}
