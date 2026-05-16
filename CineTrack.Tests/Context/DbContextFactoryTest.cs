using CineTrack.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CineTrack.Tests.Context
{
    public class DbContextFactoryTest : IDbContextFactory<CineTrackDbContext>
    {

        private readonly DbContextOptions<CineTrackDbContext> _options;

        public DbContextFactoryTest(DbContextOptions<CineTrackDbContext> options)
        {
            _options = options;
        }

        public CineTrackDbContext CreateDbContext()
        {
            return new CineTrackDbContext(_options);
        }

        public static DbContextFactoryTest Creer()
        {
            var options = new DbContextOptionsBuilder<CineTrackDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new DbContextFactoryTest(options);
        }
    }
}
