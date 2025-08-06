using FGC.Application.Common;
using FGC.Application.Sales.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace FGC.Application.Sales.Queries.GetAllSales
{
    public class GetAllSalesQuery : IRequest<List<SaleResponse>>
    {
    }
    public class GetAllSalesQueryCommandHandler
        : IRequestHandler<GetAllSalesQuery, List<SaleResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllSalesQueryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SaleResponse>> Handle(GetAllSalesQuery request,
            CancellationToken cancellationToken)
        {
           var response = await _context.Sales
           .Select(sale => new SaleResponse
            {
                Discount = sale.Discount.Value,
                StartDate = sale.StartDate,
                ExpirationDate = sale.ExpirationDate,
                Description = sale.Description
            }).ToListAsync();

            return response;
        }
    }
}
