using FGC.Application.Games.Commands.CreateGame;
using FGC.Application.UnitTests;
using FGC.Infra.Data;

public class CreateGameCommandTests : IClassFixture<TestFixture>
{
    private readonly FGC.Application.Games.Commands.CreateGame.CreateGameCommand _validator;
    private readonly ApplicationDbContext _context;

    public CreateGameCommandTests()
    {
     
    }
}
