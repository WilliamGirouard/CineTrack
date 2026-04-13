using CineTrack.Data.Context;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineTrack.Data.Repositories
{
    public class FavorisRepository : IFavorisRepository
    {
        private readonly CineTrackDbContext _context;

        public FavorisRepository(CineTrackDbContext context)
        {
            _context = context;
        }

        public async Task<List<Favoris>> GetFavorisByUserIdAsync(int userId)
        {
            return await _context.Favoris.Where(f => f.UtilisateurId == userId).ToListAsync();
        }

        public async Task AddFavorisAsync(Favoris favoris)
        {
            _context.Favoris.Add(favoris);
            await _context.SaveChangesAsync();
            Console.WriteLine(@"Favoris ajouté: " + favoris.Id);
        }

        public async Task RemoveFavorisAsync(int Id)
        {
            var favoris = await _context.Favoris.FindAsync(Id);
            if (favoris == null)
            {
                Console.WriteLine(@"Favoris non trouvé: " + Id);
                return;
            }
            else
            {
                _context.Favoris.Remove(favoris);
                await _context.SaveChangesAsync();
                Console.WriteLine(@"Favoris supprimé: " + Id);
            }
                
        }
    }
}
