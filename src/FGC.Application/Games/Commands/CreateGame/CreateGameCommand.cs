using FGC.Application.Common;
using FGC.Application.Users.Models.Response;
using FGC.Application.Games.Models.Response;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Games;

namespace FGC.Application.Games.Commands.CreateGame
{
 
    public class CreateGameCommand : IRequest<GameResponse>
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int? DealId { get; set; }
    }

    public class CreateGameCommandHandler
    : IRequestHandler<CreateGameCommand, GameResponse>
    {
        private readonly IApplicationDbContext _context;

        public CreateGameCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GameResponse> Handle(CreateGameCommand request,
            CancellationToken cancellationToken)
        {
            var game = new Game
            {
                Name = request.Name,
                DealId = request.DealId,
                Category = request.Category,
                Price = new CurrencyAmount(request.Price),
            };

            _context.Games.Add(game);

            await _context.SaveChangesAsync(cancellationToken);

            return (GameResponse)game;
        }
    }
}
