using CineTrack.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineTrack.Data.Services.Interfaces
{
    public interface IUtilisateurAnimeService
    {
        List<UtilisateurAnime> GetByUtilisateur(int utilisateurId);

        List<UtilisateurAnime> GetByMalId(int malId);

        Task AddEntryAsync(int utilisateurId, int animeId, int? note = null, string? commentaire = null);

        Task UpdateNoteAsync(int utilisateurId, int animeId, int note);

        Task UpdateCommentaireAsync(int utilisateurId, int animeId, string commentaire);

        void DeleteEntry(int utilisateurId, int animeId);
    }
}