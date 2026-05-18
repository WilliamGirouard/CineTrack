using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;

namespace CineTrack.Tests.Fakes
{
    public class FakeCommentaireRepository : ICommentaireRepository
    {
        public List<Commentaire> Commentaires { get; } = new List<Commentaire>();

        public Task<Commentaire> AddCommentaireAsync(Commentaire commentaire)
        {
            commentaire.Id = Commentaires.Count + 1;
            Commentaires.Add(commentaire);
            return Task.FromResult(commentaire);
        }

        public Task<Commentaire?> GetCommentaireAsync(int commentaireId)
        {
            return Task.FromResult(
                Commentaires.FirstOrDefault(c => c.Id == commentaireId)
            );
        }

        public Task<List<Commentaire>> GetCommentairesByAnimeIdAsync(long malId)
        {
            return Task.FromResult(
                Commentaires.Where(c => c.MalId == malId)
                            .OrderByDescending(c => c.DateCreation)
                            .ToList()
            );
        }

        public Task<List<Commentaire>> GetCommentairesByUserIdAsync(int utilisateurId)
        {
            return Task.FromResult(
                Commentaires.Where(c => c.UtilisateurId == utilisateurId)
                            .OrderByDescending(c => c.DateCreation)
                            .ToList()
            );
        }

        public Task RemoveCommentaireAsync(int commentaireId)
        {
            var commentaire = Commentaires.FirstOrDefault(c => c.Id == commentaireId);
            if (commentaire != null)
            {
                Commentaires.Remove(commentaire);
            }
            return Task.CompletedTask;
        }

        public Task<List<Commentaire>> GetCommentairesRecentAsync()
        {
            return Task.FromResult(
                Commentaires.OrderByDescending(c => c.DateCreation)
                            .Take(10)
                            .ToList()
            );
        }


        public Task<List<Commentaire>> GetAllCommentairesAsync()
        {
            return Task.FromResult(
                Commentaires.ToList()
            );
        }
    }
}
