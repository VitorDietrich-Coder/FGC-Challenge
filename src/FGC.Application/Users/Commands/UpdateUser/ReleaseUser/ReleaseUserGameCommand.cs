using FGC.Application.Common;
using System.Text.Json.Serialization;
using FGC.Domain.Entities.Users;
using FGC.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FGC.Application.Users.Commands.UpdateUser.ReleaseUserGame
{
    public class ReleaseUserGameCommand : IRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public int GameId { get; set; }

        public DateTime DateOfPurchase { get; set; } = DateTime.UtcNow;

    }


public class ReleaseUserGameCommandHandler : IRequestHandler<ReleaseUserGameCommand>
    {
        private readonly IApplicationDbContext _context;

        public ReleaseUserGameCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ReleaseUserGameCommand request, CancellationToken cancellationToken)
        {
            var exists = await _context.UserGamesLibrary
                .Where(ug => ug.UserId == request.UserId && ug.GameId == request.GameId)
                .AnyAsync(cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException($"The user {request.UserId} already has the game {request.GameId} linked.");
            }

            var game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);

            if (game == null)
            {
                throw new KeyNotFoundException($"Game with id {request.GameId} not found.");
            }

            decimal finalPrice = game.Price.Value;
            decimal discountedprice = 0;

            if (game.DealId.HasValue)
            {
                var deal = await _context.Deals
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.Id == game.DealId.Value, cancellationToken);

                if (deal != null)
                {
                    discountedprice = deal.GetDiscountPrice(game.Price.Value);
                }
                
            }

            var newUserGameLibrary = new UserGameLibrary
            {
                UserId = request.UserId,
                GameId = request.GameId,
                DateOfPurchase = request.DateOfPurchase == default ? DateTime.UtcNow : request.DateOfPurchase,
                FinalPrice = new CurrencyAmount(discountedprice == 0? game.Price.Value : discountedprice),
                CreatedAt = DateTime.UtcNow,    
            };

            _context.UserGamesLibrary.Add(newUserGameLibrary);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
