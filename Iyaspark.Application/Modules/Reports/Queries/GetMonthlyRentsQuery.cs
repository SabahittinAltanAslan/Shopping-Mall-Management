using MediatR;

namespace Iyaspark.Application.Modules.Rents.Queries
{
    public class GetMonthlyRentsQuery : IRequest<List<MonthlyRentDto>>
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public GetMonthlyRentsQuery(int year, int month)
        {
            Year = year;
            Month = month;
        }
    }
}
