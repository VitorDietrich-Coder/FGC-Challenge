using FGC.Application.Common;
using FGC.Application.Users.Models.Response;


namespace FGC.Application.Users.Queries.GetAll;

public class GetAllUsersQuery : IRequest<List<UserResponse>>
{
}
public class GetUserQueryCommandHandler
    : IRequestHandler<GetAllUsersQuery, List<UserResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetUserQueryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserResponse>> Handle(GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var response = _context.Users.Select(user => (UserResponse)user).ToList();

        return response;
    }
}

