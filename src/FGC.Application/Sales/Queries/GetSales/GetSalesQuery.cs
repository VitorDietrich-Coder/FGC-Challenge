using FGC.Application.Common;
using FGC.Application.Sales.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace FGC.Application.Sales.Queries.GetSales;

public class GetsalesQuery : IRequest<SaleResponse>
{
    public int Id { get; set; }
}
public class GetsalesQueryCommandHandler
    : IRequestHandler<GetsalesQuery, SaleResponse>
{
    private readonly IApplicationDbContext _context;

    public GetsalesQueryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SaleResponse> Handle(GetsalesQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Sales.Where(x => x.Id == request.Id).FirstAsync();
 
        return (SaleResponse)entity;
    }
}