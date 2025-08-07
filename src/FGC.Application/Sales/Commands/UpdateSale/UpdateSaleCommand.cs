using FGC.Application.Common;
using FGC.Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace FGC.Application.Sales.Commands.UpdateSale;

public class UpdateSaleCommand : IRequest
{
    [JsonIgnore]
    public int Id { get; set; } 
    public decimal? Discount { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public List<int?>? GameId { get; set; }
    public string Description { get; set; }
}

public class UpdateSaleCommandHandler
    : AsyncRequestHandler<UpdateSaleCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSaleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task Handle(UpdateSaleCommand request,
        CancellationToken cancellationToken)
    {
        var sale = await _context.Sales.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

        if (sale == null)
        {
            throw new KeyNotFoundException("Sale not found.");
        }

        sale.Description = request.Description;
        sale.UpdateDiscount(request.Discount, request.ExpirationDate);
       
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);


        var validIds = request.GameId?
               .Where(id => id.HasValue)
               .Select(id => id.Value)
               .ToList();

        if (validIds == null || validIds.Count == 0)
              return;

        var games = new List<Game>();

        foreach (var gameId in validIds)
        {
            var game = await _context.Games.Where(x => x.Id == gameId).FirstOrDefaultAsync();
            if (game is null)
            {
                continue;
            }

            game.UpdateSale(sale.Id);
            games.Add(game);
        }

        if (games.Count > 0)
             _context.Games.UpdateRange(games);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
