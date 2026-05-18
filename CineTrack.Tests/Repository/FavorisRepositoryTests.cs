using CineTrack.Data.Models;
using CineTrack.Data.Repositories;
using CineTrack.Tests.Context;
using Microsoft.EntityFrameworkCore;

namespace CineTrack.Tests.Repository
{
    public class FavorisRepositoryTests
    {
        [Fact]
        public async Task AddFavorisAsync_NouveauFavoris_EstPersiste()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new FavorisRepository(factory);

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });

            using var context = factory.CreateDbContext();
            Assert.Equal(1, await context.Favoris.CountAsync());
        }

        [Fact]
        public async Task GetFavorisByUserIdAsync_FavorisPresent_RetourneLaListe()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new FavorisRepository(factory);

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });
            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 1535 });

            var result = await repo.GetFavorisByUserIdAsync(1);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetFavorisByUserIdAsync_AucunFavoris_RetourneListeVide()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new FavorisRepository(factory);

            var result = await repo.GetFavorisByUserIdAsync(1);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFavorisByUserIdAsync_AutreUtilisateur_NeRetournePasSesFavoris()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new FavorisRepository(factory);

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });

            var result = await repo.GetFavorisByUserIdAsync(2);

            Assert.Empty(result);
        }

        [Fact]
        public async Task RemoveFavorisAsync_FavorisPresent_EstSupprime()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new FavorisRepository(factory);

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });

            using var context = factory.CreateDbContext();
            var favoris = await context.Favoris.FirstAsync();

            await repo.RemoveFavorisAsync(favoris.Id);

            Assert.Equal(0, await context.Favoris.CountAsync());
        }

        [Fact]
        public async Task RemoveFavorisAsync_FavorisAbsent_SuppressionNonFaite()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new FavorisRepository(factory);

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });

            await repo.RemoveFavorisAsync(666);

            using var context = factory.CreateDbContext();
            Assert.Equal(1, await context.Favoris.CountAsync());
        }

        [Fact]
        public async Task AddFavorisAsync_PlusieursFavoris_TousPersistes()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new FavorisRepository(factory);

            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 21 });
            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 1535 });
            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 2, MalId = 21 });

            using var context = factory.CreateDbContext();
            Assert.Equal(3, await context.Favoris.CountAsync());
        }
    }
}