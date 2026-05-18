using CineTrack.Data.Models;

namespace CineTrack.Tests.Fakes
{
    public class FakeFavorisRepositoryTests
    {
        [Fact]
        public async Task AddFavorisAsync_NouveauFavoris_EstAjouteListe()
        {
            var repo = new FakeFavorisRepository();

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });

            Assert.Single(repo.Favoris);
            Assert.Equal(21, repo.Favoris[0].MalId);
        }

        [Fact]
        public async Task AddFavorisAsync_NouveauFavoris_IdAutoIncrement()
        {
            var repo = new FakeFavorisRepository();

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });
            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 1535 });

            Assert.Equal(1, repo.Favoris[0].Id);
            Assert.Equal(2, repo.Favoris[1].Id);
        }

        [Fact]
        public async Task GetFavorisByUserIdAsync_FavorisPresent_RetourneLaListe()
        {
            var repo = new FakeFavorisRepository();

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });
            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 1535 });

            var result = await repo.GetFavorisByUserIdAsync(1);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetFavorisByUserIdAsync_AucunFavoris_RetourneListeVide()
        {
            var repo = new FakeFavorisRepository();

            var result = await repo.GetFavorisByUserIdAsync(1);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFavorisByUserIdAsync_AutreUtilisateur_NeRetournePasSesFavoris()
        {
            var repo = new FakeFavorisRepository();

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });

            var result = await repo.GetFavorisByUserIdAsync(2);

            Assert.Empty(result);
        }

        [Fact]
        public async Task RemoveFavorisAsync_FavorisPresent_EstSupprime()
        {
            var repo = new FakeFavorisRepository();

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });
            int id = repo.Favoris[0].Id;

            await repo.RemoveFavorisAsync(id);

            Assert.Empty(repo.Favoris);
        }

        [Fact]
        public async Task RemoveFavorisAsync_FavorisAbsent_SuppressionNonFaite()
        {
            var repo = new FakeFavorisRepository();

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });

            await repo.RemoveFavorisAsync(666);

            Assert.Single(repo.Favoris);
        }

        [Fact]
        public async Task AddFavorisAsync_PlusieursFavoris_TousPersistes()
        {
            var repo = new FakeFavorisRepository();

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });
            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 1535 });
            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 2, MalId = 21 });

            Assert.Equal(3, repo.Favoris.Count);
        }
    }
}