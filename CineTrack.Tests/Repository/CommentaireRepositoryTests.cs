using CineTrack.Data.Models;
using CineTrack.Tests.Fakes;

namespace CineTrack.Tests.Repository
{
    public class CommentaireRepositoryTests
    {
        [Fact]
        public async Task AddCommentaireAsync_NouveauCommentaire_EstPersiste()
        {
            var repo = new FakeCommentaireRepository();

            await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 1, 
                    MalId = 1, 
                    Texte = "Super anime!", 
                    DateCreation = DateTime.Now 
                }
            );

            Assert.Single(repo.Commentaires);
        }

        [Fact]
        public async Task GetCommentairesByAnimeIdAsync_CommentairesDifferentsAnimes_RetourneSeulementBonAnime()
        {
            var repo = new FakeCommentaireRepository();

            await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 1, 
                    MalId = 42, 
                    Texte = "Commentaire anime 42", 
                    DateCreation = DateTime.Now 
                }
            );

            await repo.AddCommentaireAsync(
                new Commentaire 
                { 
                    UtilisateurId = 1, 
                    MalId = 99, 
                    Texte = "Commentaire anime 99", 
                    DateCreation = DateTime.Now 
                }
            );

            var results = await repo.GetCommentairesByAnimeIdAsync(42);

            Assert.Single(results);
        }
    }
}