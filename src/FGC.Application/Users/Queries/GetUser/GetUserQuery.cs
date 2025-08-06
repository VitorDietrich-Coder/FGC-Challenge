using FGC.Application.Common;
using FGC.Application.Users.Models.Response;
using FGC.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace FGC.Application.Users.Queries.GetUser;

public class GetUserQuery : IRequest<UserResponse>
{
    public int Id { get; set; }
}
public class GetUserQueryCommandHandler
    : IRequestHandler<GetUserQuery, UserResponse>
{
    private readonly IApplicationDbContext _context;

    public GetUserQueryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserResponse> Handle(GetUserQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Users.Where(x => x.Id == request.Id).FirstAsync();
 
        return (UserResponse)entity;
    }
}