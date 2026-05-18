using CineTrack.Data.Models;

namespace CineTrack.Tests.Fakes
{
    public class FakeCommentaireRepositoryTests
    {
        [Fact]
        public async Task AddCommentaireAsync_NouveauCommentaire_IdAutoIncrement()
        {
            var repo = new FakeCommentaireRepository();

            var first = await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 1, 
                    MalId = 1, 
                    Texte = "Premier", 
                    DateCreation = DateTime.Now 
                }
            );

            var second = await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 1, 
                    MalId = 1, 
                    Texte = "Deuxième", 
                    DateCreation = DateTime.Now 
                }
            );

            Assert.Equal(1, first.Id);
            Assert.Equal(2, second.Id);
        }

        [Fact]
        public async Task GetCommentaireAsync_CommentairePresent_RetourneCommentaire()
        {
            var repo = new FakeCommentaireRepository();

            var added = await repo.AddCommentaireAsync(
                new Commentaire 
                {
                    UtilisateurId = 1, 
                    MalId = 1, 
                    Texte = "Super anime!", 
                    DateCreation = DateTime.Now 
                }
            );

            var result = await repo.GetCommentaireAsync(added.Id);

            Assert.NotNull(result);
            Assert.Equal(added.Id, result.Id);
        }

        [Fact]
        public async Task GetCommentaireAsync_CommentaireAbsent_RetourneNull()
        {
            var repo = new FakeCommentaireRepository();

            var result = await repo.GetCommentaireAsync(9999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetCommentairesByAnimeIdAsync_CommentairesDifferentsAnimes_RetourneBonAnime()
        {
            var repo = new FakeCommentaireRepository();
            
            await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 1, 
                    MalId = 42, 
                    Texte = "Anime 42", 
                    DateCreation = DateTime.Now 
                }
            );
            
            await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 1, 
                    MalId = 99, 
                    Texte = "Anime 99", 
                    DateCreation = DateTime.Now 
                }
            );

            var results = await repo.GetCommentairesByAnimeIdAsync(42);

            Assert.Single(results);
        }

        [Fact]
        public async Task GetCommentairesByUserIdAsync_CommentairesDifferentsUtilisateurs_RetourneBonUtilisateur()
        {
            var repo = new FakeCommentaireRepository();
            
            await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 1, 
                    MalId = 1, 
                    Texte = "User 1", 
                    DateCreation = DateTime.Now 
                }
            );
            
            await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 2, 
                    MalId = 1, 
                    Texte = "User 2", 
                    DateCreation = DateTime.Now 
                }
            );

            var results = await repo.GetCommentairesByUserIdAsync(1);

            Assert.Single(results);
        }

        [Fact]
        public async Task RemoveCommentaireAsync_CommentaireExistant_EstSupprime()
        {
            var repo = new FakeCommentaireRepository();

            var added = await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 1, 
                    MalId = 1, 
                    Texte = "Super anime!", 
                    DateCreation = DateTime.Now 
                }
            );

            await repo.RemoveCommentaireAsync(added.Id);

            Assert.Empty(repo.Commentaires);
        }

        [Fact]
        public async Task RemoveCommentaireAsync_IdInexistant_NeLancePasException()
        {
            var repo = new FakeCommentaireRepository();

            var exception = await Record.ExceptionAsync(() => repo.RemoveCommentaireAsync(9999));

            Assert.Null(exception);
        }

        [Fact]
        public async Task GetCommentairesRecentAsync_PlusDe10Commentaires_RetourneMax10()
        {
            var repo = new FakeCommentaireRepository();

            for (int i = 0; i < 15; i++)
            {
                await repo.AddCommentaireAsync(
                    new Commentaire 
                    { 
                        UtilisateurId = 1, 
                        MalId = 1, 
                        Texte = $"Commentaire {i}", 
                        DateCreation = DateTime.Now 
                    }
                );
            }

            var results = await repo.GetCommentairesRecentAsync();

            Assert.Equal(10, results.Count);
        }

        [Fact]
        public async Task GetAllCommentairesAsync_PlusieursCommentaires_RetourneTout()
        {
            var repo = new FakeCommentaireRepository();
            
            await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 1, 
                    MalId = 1, 
                    Texte = "Un", 
                    DateCreation = DateTime.Now 
                }
            );
            
            await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 1, 
                    MalId = 1, 
                    Texte = "Deux", 
                    DateCreation = DateTime.Now 
                }
            );
            
            await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 1, 
                    MalId = 1, 
                    Texte = "Trois", 
                    DateCreation = DateTime.Now 
                }
            );

            var results = await repo.GetAllCommentairesAsync();

            Assert.Equal(3, results.Count);
        }
    }
}