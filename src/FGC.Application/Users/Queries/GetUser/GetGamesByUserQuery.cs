using FGC.Application.Common;
using FGC.Application.Users.Models.Response;


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
        var entity = _context.UserGamesLibrary.Where(x => x.UserId == request.Id).Select(lg => (UserLibraryGameResponse)lg).ToList();

        return entity;
    }
}

