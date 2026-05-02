using CineTrack.Data.Models;
namespace CineTrack.Data.Repositories.Interfaces
{
    public interface ICommentaireRepository
    {
        Task<List<Commentaire>> GetCommentairesByAnimeIdAsync(long malId);
        Task<Commentaire> AddCommentaireAsync(Commentaire commentaire);
        Task<Commentaire?> GetCommentaireAsync(int commentaireId);
        Task RemoveCommentaireAsync(int commentaireId);
        Task<List<Commentaire>> GetCommentairesRecentAsync();
    }
}
