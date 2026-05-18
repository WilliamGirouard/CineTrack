using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;

namespace CineTrack.Tests.Fakes
{
    public class FakeFavorisRepository : IFavorisRepository
    {
        public List<Favoris> Favoris { get; } = new List<Favoris>();
        private int _nextId = 1;

        public Task AddFavorisAsync(Favoris favoris)
        {
            favoris.Id = _nextId++;
            Favoris.Add(favoris);
            return Task.CompletedTask;
        }

        public Task RemoveFavorisAsync(int id)
        {
            var favoris = Favoris.FirstOrDefault(f => f.Id == id);
            if (favoris != null)
                Favoris.Remove(favoris);
            return Task.CompletedTask;
        }

        public Task<List<Favoris>> GetFavorisByUserIdAsync(int userId)
            => Task.FromResult(Favoris.Where(f => f.UtilisateurId == userId).ToList());
    }
}