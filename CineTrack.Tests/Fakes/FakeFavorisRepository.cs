using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Models;

namespace CineTrack.Tests.Fakes
{
    public class FakeFavorisRepository : IFavorisRepository
    {
        private readonly List<Favoris> _favoris = new();
        private int _currentId = 1;
        public List<Favoris> FavorisList => _favoris;

        public Task AddFavorisAsync(Favoris favoris)
        {
            favoris.Id = _currentId++;
            _favoris.Add(favoris);
            Console.WriteLine($"Favoris ajouté: {favoris.Id}");
            return Task.CompletedTask;
        }

        public Task RemoveFavorisAsync(int Id)
        {
            var favoris = _favoris.FirstOrDefault(f => f.Id == Id);
            if (favoris == null)
            {
                Console.WriteLine($"Favoris non trouvé: {Id}");
                return Task.CompletedTask;
            }
            _favoris.Remove(favoris);
            Console.WriteLine($"Favoris supprimé: {Id}");
            return Task.CompletedTask;
        }

        public Task<List<Favoris>> GetFavorisByUserIdAsync(int userId)
        {
            var result = _favoris.Where(f => f.UtilisateurId == userId).ToList();
            return Task.FromResult(result);
        }
    }
}
