﻿using CineTrack.Data.Models;

namespace CineTrack.Tests.Fakes
{
    public class FakeFavorisRepositoryTests
    {
        [Fact]
        public async Task AddFavorisAsync_NouveauFavoris_EstAjouteListe()
        {
            var repo = new FakeFavorisRepository();
            var favoris = new Favoris { UtilisateurId = 1, MalId = 100 };
            await repo.AddFavorisAsync(favoris);
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

        [Fact]
        public async Task RemoveFavorisAsync_IdInexistant_NeLancePasException()
        {
            var repo = new FakeFavorisRepository();
            var exception = await Record.ExceptionAsync(() => repo.RemoveFavorisAsync(9999));
            Assert.Null(exception);
        }


        // Retourne un seul favoris pour un utilisateur donné
        [Fact]
        public async Task GetFavorisByUserIdAsync_FavorisPresent_RetourneFavoris()
        {
            var repo = new FakeFavorisRepository();
            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 100 });
            var result = await repo.GetFavorisByUserIdAsync(1);
            Assert.Single(result);
            Assert.Equal(100, result[0].MalId);
        }

        // Retourne plusieurs favoris pour un utilisateur donné
        [Fact]
        public async Task GetFavorisByUserIdAsync_FavorisPresent_RetourneListe()
        {
            var repo = new FakeFavorisRepository();
            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 100 });
            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 1, MalId = 101 });
            await repo.AddFavorisAsync(new Favoris { UtilisateurId = 2, MalId = 100 });
            var result = await repo.GetFavorisByUserIdAsync(1);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, f => f.MalId == 100);
            Assert.Contains(result, f => f.MalId == 101);
        }

        // Test pour vérifier que GetFavorisByUserIdAsync retourne une liste vide si aucun favoris n'est trouvé pour l'utilisateur donné
        [Fact]
        public async Task GetFavorisByUserIdAsync_AucunFavoris_RetourneListeVide()
        {
            var repo = new FakeFavorisRepository();
            var result = await repo.GetFavorisByUserIdAsync(1);
            Assert.Empty(result);
        }

    }
}