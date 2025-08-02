using Iyaspark.Application.Modules.Reports.DTOs;

namespace Iyaspark.Application.Interfaces.Services
{
    public interface IExcelExportService
    {
        byte[] GenerateProfitabilityReportExcel(List<ProfitabilityReportDto> data);
        byte[] GenerateRevenueReportExcel(List<RevenueReportDto> data);
    }
}
