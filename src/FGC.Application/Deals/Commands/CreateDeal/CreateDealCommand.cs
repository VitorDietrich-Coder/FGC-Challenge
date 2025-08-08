using FGC.Application.Common;
using FGC.Application.Deals.Models.Response;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Games;
using FGC.Domain.Entities.Deals;
using Microsoft.EntityFrameworkCore;

namespace FGC.Application.Deals.Commands.CreateDeal
{
    public class CreateDealCommand : IRequest<DealResponse>
    {
        public decimal Discount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<int?>? GameId { get; set; }
        public string Description { get; set; }
    }

    public class CreateDealCommandHandler : IRequestHandler<CreateDealCommand, DealResponse>
    {
        private readonly IApplicationDbContext _context;

        public CreateDealCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DealResponse> Handle(CreateDealCommand request,
            CancellationToken cancellationToken)
        {
             var deal = new Deal()
             {
                Discount = new CurrencyAmount(request.Discount, "BRL"),
                ExpirationDate = new DateUtc(request.ExpirationDate),
                StartDate = new DateUtc(DateTime.UtcNow),
                Description = request.Description,
             };

            _context.Deals.Add(deal);
            await _context.SaveChangesAsync(cancellationToken);

            var validIds = request.GameId
                 .Where(id => id.HasValue)
                 .Select(id => id.Value)
                 .ToList();

            var games = new List<Game>();

            foreach (var gameId in validIds)
            {
                var game = await _context.Games.Where(x => x.Id == gameId).FirstOrDefaultAsync();
                if (game is null)
                {
                     continue;
                }

                game.DealId = deal.Id;
                games.Add(game);
            }

            if (games.Count != 0)
            {
                 _context.Games.UpdateRange(games);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return (DealResponse)deal;
        }
    }
}
