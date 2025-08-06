using FGC.Application.Common;
using FGC.Application.Sales.Models.Response;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.GameAggregate;
using FGC.Domain.SaleAggregate;
using Microsoft.EntityFrameworkCore;

namespace FGC.Application.Sales.Commands.CreateSale
{
    public class CreateSaleCommand : IRequest<SaleResponse>
    {
        public decimal Discount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<int?>? GameId { get; set; }
        public string Description { get; set; }
    }

    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleResponse>
    {
        private readonly IApplicationDbContext _context;

        public CreateSaleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SaleResponse> Handle(CreateSaleCommand request,
            CancellationToken cancellationToken)
        {
             var sale = new Sale()
             {
                Discount = new CurrencyAmount(request.Discount, "BRL"),
                ExpirationDate = new DateUtc(request.ExpirationDate),
                StartDate = new DateUtc(DateTime.UtcNow),
                Description = request.Description,
             };

            _context.Sales.Add(sale);
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

                game.SaleID = sale.Id;
                games.Add(game);
            }

            if (games.Count != 0)
            {
                 _context.Games.UpdateRange(games);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return (SaleResponse)sale;
        }
    }
}
