using FGC.Application.Common;
using FGC.Application.Deals.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace FGC.Application.Deals.Queries.Getdeals;

public class GetDealsByIdQuery : IRequest<DealResponse>
{
    public int Id { get; set; }
}
public class GetDealsQueryCommandHandler
    : IRequestHandler<GetDealsByIdQuery, DealResponse>
{
    private readonly IApplicationDbContext _context;

    public GetDealsQueryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DealResponse> Handle(GetDealsByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Deals.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

        if (entity == null)
        {
            throw new NotFoundException(nameof(Deals), request.Id.ToString());
        }

        return (DealResponse)entity;
    }
}