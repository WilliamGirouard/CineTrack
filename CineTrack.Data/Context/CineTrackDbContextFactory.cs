using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace CineTrack.Data.Context
{
    public class CineTrackDbContextFactory : IDesignTimeDbContextFactory<CineTrackDbContext>
    {
        public CineTrackDbContext CreateDbContext(string[] args)
        {
           var optionsBuilder = new DbContextOptionsBuilder<CineTrackDbContext>();
            optionsBuilder.UseSqlite("Data Source=CineTrack.db");
            return new CineTrackDbContext(optionsBuilder.Options);
        }
        // MDR j'ai les memes probleme que ce bon Jason Aller ya 6 ans
        // https://stackoverflow.com/questions/60561851/an-error-occurred-while-accessing-the-microsoft-extensions-hosting-services-when
    }
}
