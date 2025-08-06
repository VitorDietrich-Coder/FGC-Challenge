using FGC.Application.Common;
using FGC.Application.Games.Models.Response;


namespace FGC.Application.Games.Queries.GetAllGames
{
 
    public class GetAllGamesQuery : IRequest<List<GameResponse>>
    {
    }
    public class GetUserQueryCommandHandler
        : IRequestHandler<GetAllGamesQuery, List<GameResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetUserQueryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GameResponse>> Handle(GetAllGamesQuery request,
            CancellationToken cancellationToken)
        {
            var response = _context.Games.Select(game => (GameResponse)game).ToList();

            return response;
        }
    }

}
