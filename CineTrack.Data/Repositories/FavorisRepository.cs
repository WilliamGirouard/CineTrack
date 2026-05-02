using CineTrack.Data.Context;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineTrack.Data.Repositories
{
    public class FavorisRepository : IFavorisRepository
    {
        private readonly IDbContextFactory<CineTrackDbContext> _context;

        public FavorisRepository(IDbContextFactory<CineTrackDbContext> context)
        {
            _context = context;
        }

        public async Task<List<Favoris>> GetFavorisByUserIdAsync(int userId)
        {
            using var context = _context.CreateDbContext();
            return await context.Favoris.Where(f => f.UtilisateurId == userId).ToListAsync();
        }

        public async Task AddFavorisAsync(Favoris favoris)
        {
            using var context = _context.CreateDbContext();
            context.Favoris.Add(favoris);
            await context.SaveChangesAsync();
            Console.WriteLine(@"Favoris ajouté: " + favoris.Id);
        }

        public async Task RemoveFavorisAsync(int Id)
        {
            using var context = _context.CreateDbContext();
            var favoris = await context.Favoris.FindAsync(Id);
            if (favoris == null)
            {
                Console.WriteLine(@"Favoris non trouvé: " + Id);
                return;
            }
            else
            {
                context.Favoris.Remove(favoris);
                await context.SaveChangesAsync();
                Console.WriteLine(@"Favoris supprimé: " + Id);
            }
                
        }
    }
}
