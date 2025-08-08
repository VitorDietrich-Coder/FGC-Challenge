using FGC.Infra.Data;
using Microsoft.Extensions.Configuration;


namespace FGC.Application.UnitTests
{
    [CollectionDefinition(nameof(ReadOnlyCollection))]
    public class ReadOnlyCollection : ICollectionFixture<TestFixture> { }

    public class TestFixture : IDisposable
    {
        private readonly DbContextFactory _contextFactory;
        public IConfiguration Configuration { get; }

        // Test Setup
        public TestFixture()
        {
            Configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.Test.json", optional: false)
                   .Build();
            _contextFactory = new DbContextFactory();
            Context = _contextFactory.Create();
         }

        public ApplicationDbContext Context { get; }
 
        // Test Cleanup
        public void Dispose()
        {
            _contextFactory.Dispose();
        }
    }
}
