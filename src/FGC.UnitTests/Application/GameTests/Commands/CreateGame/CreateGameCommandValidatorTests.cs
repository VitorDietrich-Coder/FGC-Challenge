using FGC.Application.Games.Commands.CreateGame;
using FGC.Application.UnitTests;
using FGC.Infra.Data;

public class CreateGameCommandValidatorTests : IClassFixture<TestFixture>
{
    private readonly CreateGameCommand _validator;
    private readonly ApplicationDbContext _context;

    public CreateGameCommandValidatorTests()
    {
     
    }
}
