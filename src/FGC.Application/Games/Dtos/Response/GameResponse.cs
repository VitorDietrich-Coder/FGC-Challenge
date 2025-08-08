using FGC.Domain.Entities.Games;

namespace FGC.Application.Games.Models.Response
{
    
    public record GameResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int? DealId { get; set; }

        public static explicit operator GameResponse(Game game)
        {
            return new GameResponse
            {
                Id = game.Id,
                Name = game.Name,
                Category = game.Category,
                Price = game.Price.Value,
                DealId = game.DealId
            };
        }
    }
}
