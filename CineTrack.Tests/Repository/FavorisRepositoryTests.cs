using CineTrack.Data.Models;
using CineTrack.Tests.Fakes;

namespace CineTrack.Tests.Repository
{
    public class FavorisRepositoryTests
    {
        [Fact]
        public async Task AddFavorisAsync_NouveauFavoris_EstAjouteListe()
        {
            var repo = new FakeFavorisRepository();

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 100 });
            Assert.Single(repo.FavorisList);
            Assert.Equal(1, repo.FavorisList[0].UtilisateurId);
            Assert.Equal(100, repo.FavorisList[0].MalId);
        }

        [Fact]
        public async Task RemoveFavorisAsync_FavorisExistant_EstSupprime()
        {
            var repo = new FakeFavorisRepository();
            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 100 });
            await repo.RemoveFavorisAsync(1);
            Assert.Empty(repo.FavorisList);
        }
    }
}
