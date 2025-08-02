namespace Iyaspark.Application.Modules.Reports.Requests
{
    public class ReportFilterRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? Sector { get; set; }
        public string? FacadeDirection { get; set; }
        public string? TenantType { get; set; }
        public string? FloorLabel { get; set; }
        public bool? HasExtraStorage { get; set; }
        public decimal? SquareMeter { get; set; }
    }
}
