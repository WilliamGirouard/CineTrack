using CineTrack.Data.Context;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;

namespace CineTrack.Data.Repositories
{
    public class FavorisRepository : IFavorisRepository
    {
        private readonly CineTrackDbContext _context;

        public FavorisRepository(CineTrackDbContext context)
        {
            _context = context;
        }

        public List<Favoris> GetFavorisByUserId(int userId)
        {
            return _context.Favoris.Where(f => f.UtilisateurId == userId).ToList();
        }

        public void AddFavoris(Favoris favoris)
        {
            _context.Favoris.Add(favoris);
            _context.SaveChanges();
            Console.WriteLine(@"Favoris ajouté: " + favoris.Id);
        }

        public void RemoveFavoris(int Id)
        {
            var favoris = _context.Favoris.Find(Id);
            if (favoris == null)
            {
                Console.WriteLine(@"Favoris non trouvé: " + Id);
                return;
            }
            else
            {
                _context.Favoris.Remove(favoris);
                _context.SaveChanges();
                Console.WriteLine(@"Favoris supprimé: " + Id);
            }
                
        }
    }
}
