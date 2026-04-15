using CineTrack.Data.Models;
namespace CineTrack.Data.Repositories.Interfaces
{
    public interface ICommentaireRepository
    {
        Task<List<Commentaire>> GetCommentairesByAnimeIdAsync(int animeId);
        Task<Commentaire> AddCommentaireAsync(Commentaire commentaire);
        Task RemoveCommentaireAsync(int id);
    }
}
