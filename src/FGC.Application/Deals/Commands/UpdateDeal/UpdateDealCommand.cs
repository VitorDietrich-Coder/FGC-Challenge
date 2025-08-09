using FGC.Application.Common;
using FGC.Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace FGC.Application.Deals.Commands.UpdateDeal;

public class UpdateDealCommand : IRequest<Unit>
{
    [JsonIgnore]
    public int Id { get; set; } 
    public decimal? Discount { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public List<int?>? GameId { get; set; }
    public string Description { get; set; }
}

public class UpdateDealCommandHandler
    : IRequestHandler<UpdateDealCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateDealCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDealCommand request,
        CancellationToken cancellationToken)
    {
        var deal = await _context.Deals.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

        if (deal == null)
        {
            throw new KeyNotFoundException("Deal not found.");
        }

        deal.Description = request.Description;
        deal.UpdateDiscount(request.Discount, request.ExpirationDate, DateTime.UtcNow);
       
        _context.Deals.Update(deal);
        await _context.SaveChangesAsync(cancellationToken);


        var validIds = request.GameId?
               .Where(id => id.HasValue)
               .Select(id => id.Value)
               .ToList();

        if (validIds == null || validIds.Count == 0)
              return new Unit();

        var games = new List<Game>();

        foreach (var gameId in validIds)
        {
            var game = await _context.Games.Where(x => x.Id == gameId).FirstOrDefaultAsync();
            if (game is null)
            {
                continue;
            }

            game.UpdateDeal(deal.Id);
            games.Add(game);
        }

        if (games.Count > 0)
             _context.Games.UpdateRange(games);

        await _context.SaveChangesAsync(cancellationToken);

        return new Unit();
    }
}
