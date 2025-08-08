using FGC.Application.Common;
using FGC.Application.Deals.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace FGC.Application.Deals.Queries.GetAlldeals
{
    public class GetAllDealsQuery : IRequest<List<DealResponse>>
    {
    }
    public class GetAllDealsQueryCommandHandler
        : IRequestHandler<GetAllDealsQuery, List<DealResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllDealsQueryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DealResponse>> Handle(GetAllDealsQuery request,
            CancellationToken cancellationToken)
        {
           var response = await _context.Deals
           .Select(deal => new DealResponse
           {
                DealId = deal.Id,
                Discount = deal.Discount.Value,
                StartDate = deal.StartDate,
                ExpirationDate = deal.ExpirationDate,
                Description = deal.Description
            }).ToListAsync();

            return response;
        }
    }
}
