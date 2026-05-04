using CineTrack.Data.Models;

namespace CineTrack.Data.Repositories.Interfaces;

public interface INoteRepository
{
    Task<Note?> GetNoteByUserIdAndAnimeIdAsync(int utilisateurId, long malId);
    Task<List<Note>> GetNotesByAnimeIdAsync(long malId);
    Task<List<Note>> GetNotesByUserIdAsync(int utilisateurId);
    Task AddNoteAsync(Note note);
    Task UpdateNoteAsync(Note note);
    Task<Note?> GetNoteByIdAsync(int noteId);
    Task DeleteNoteByIdAsync(int noteId);
    Task<List<Note>> GetNotesRecentesAsync();
}
