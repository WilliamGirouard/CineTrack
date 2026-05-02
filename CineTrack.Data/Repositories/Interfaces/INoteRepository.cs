using CineTrack.Data.Models;

namespace CineTrack.Data.Repositories.Interfaces;

public interface INoteRepository
{
    Task<Note?> GetNoteByUserAndAnime(int utilisateurId, long malId);
    Task<List<Note>> GetNotesByAnimeAsync(long malId);
    Task AddNoteAsync(Note note);
    Task UpdateNoteAsync(Note note);
}