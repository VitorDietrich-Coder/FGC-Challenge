using FGC.Application.Common;
using FGC.Application.Games.Models.Response;
using Microsoft.EntityFrameworkCore;


namespace FGC.Application.Games.Queries.GetGames
{
 
    public class GetGameByIdQuery : IRequest<GameResponse>
    {
        public int Id { get; set; }
    }
    public class GetGameQueryCommandHandler
        : IRequestHandler<GetGameByIdQuery, GameResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetGameQueryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GameResponse> Handle(GetGameByIdQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _context.Games.Where(x => x.Id == request.Id).FirstAsync();

            return (GameResponse)entity;
        }
    }
}
