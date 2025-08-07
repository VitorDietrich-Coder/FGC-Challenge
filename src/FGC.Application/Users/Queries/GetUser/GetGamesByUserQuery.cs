using FGC.Application.Common;
using FGC.Application.Users.Models.Response;
using Microsoft.EntityFrameworkCore;


namespace FGC.Application.Users.Queries.GetUser;

public class GetGamesByUserQuery : IRequest<List<UserLibraryGameResponse>>
{
    public int Id { get; set; }
}
public class GetGamesByUserQueryCommandHandler
    : IRequestHandler<GetGamesByUserQuery, List<UserLibraryGameResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetGamesByUserQueryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserLibraryGameResponse>> Handle(GetGamesByUserQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.UserGamesLibrary
           .Where(x => x.UserId == request.Id)
           .Select(lg => new UserLibraryGameResponse
           {
               GameId = lg.Game.Id,
               Name = lg.Game.Name,
               Category = lg.Game.Category,
               BasePrice = lg.Game.Price.Value,
               BaseCurrency = lg.Game.Price.Currency,
               FinalPrice = lg.FinalPrice.Value,
               PurchaseCurrency = lg.FinalPrice.Currency,
               PurchaseDate = lg.DateOfPurchase
           })
           .ToListAsync();

        return entity;
    }
}

