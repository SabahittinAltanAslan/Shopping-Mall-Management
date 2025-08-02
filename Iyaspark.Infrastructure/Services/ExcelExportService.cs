using ClosedXML.Excel;
using Iyaspark.Application.Interfaces.Services;
using Iyaspark.Application.Modules.Reports.DTOs;

namespace Iyaspark.Infrastructure.Services
{
    public class ExcelExportService : IExcelExportService
    {
        public byte[] GenerateProfitabilityReportExcel(List<ProfitabilityReportDto> data)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Karlılık Raporu");

            worksheet.Cell(1, 1).Value = "Firma Adı";
            worksheet.Cell(1, 2).Value = "Vergi No";
            worksheet.Cell(1, 3).Value = "Sektör";
            worksheet.Cell(1, 4).Value = "Cephe";
            worksheet.Cell(1, 5).Value = "Tip";
            worksheet.Cell(1, 6).Value = "Kod";
            worksheet.Cell(1, 7).Value = "Kat";
            worksheet.Cell(1, 8).Value = "m²";
            worksheet.Cell(1, 9).Value = "Kira Tipi";
            worksheet.Cell(1, 10).Value = "Sabit Kira";
            worksheet.Cell(1, 11).Value = "Ciro Bazlı Kira";
            worksheet.Cell(1, 12).Value = "Ekstra Depo Kirası";
            worksheet.Cell(1, 13).Value = "Toplam Ciro";
            worksheet.Cell(1, 14).Value = "Toplam Kira";
            worksheet.Cell(1, 15).Value = "Kâr";

            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.CompanyName;
                worksheet.Cell(row, 2).Value = item.TaxNumber;
                worksheet.Cell(row, 3).Value = item.Sector;
                worksheet.Cell(row, 4).Value = item.FacadeDirection;
                worksheet.Cell(row, 5).Value = item.TenantType;
                worksheet.Cell(row, 6).Value = item.FloorCode;
                worksheet.Cell(row, 7).Value = item.FloorLabel;
                worksheet.Cell(row, 8).Value = item.SquareMeter;
                worksheet.Cell(row, 9).Value = item.RentType;
                worksheet.Cell(row, 10).Value = item.FixedRentAmount;
                worksheet.Cell(row, 11).Value = item.RevenueBasedRent;
                worksheet.Cell(row, 12).Value = item.ExtraStorageRent;
                worksheet.Cell(row, 13).Value = item.TotalRevenue;
                worksheet.Cell(row, 14).Value = item.TotalRent;
                worksheet.Cell(row, 15).Value = item.Profit;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        public byte[] GenerateRevenueReportExcel(List<RevenueReportDto> data)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Ciro Raporu");

            // Başlıklar
            worksheet.Cell(1, 1).Value = "Firma Adı";
            worksheet.Cell(1, 2).Value = "Vergi No";
            worksheet.Cell(1, 3).Value = "Sektör";
            worksheet.Cell(1, 4).Value = "Cephe";
            worksheet.Cell(1, 5).Value = "Tip";
            worksheet.Cell(1, 6).Value = "Kodu";
            worksheet.Cell(1, 7).Value = "Kat";
            worksheet.Cell(1, 8).Value = "m²";
            worksheet.Cell(1, 9).Value = "Toplam Ciro";

            // Satırlar
            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.CompanyName;
                worksheet.Cell(row, 2).Value = item.TaxNumber;
                worksheet.Cell(row, 3).Value = item.Sector;
                worksheet.Cell(row, 4).Value = item.FacadeDirection;
                worksheet.Cell(row, 5).Value = item.TenantType;
                worksheet.Cell(row, 6).Value = item.FloorCode;
                worksheet.Cell(row, 7).Value = item.FloorLabel;
                worksheet.Cell(row, 8).Value = item.SquareMeter;
                worksheet.Cell(row, 9).Value = item.TotalRevenue;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }

}
