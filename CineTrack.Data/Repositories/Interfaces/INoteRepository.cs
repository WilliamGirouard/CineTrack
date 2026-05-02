using CineTrack.Data.Models;

namespace CineTrack.Data.Repositories.Interfaces;

public interface INoteRepository
{
    Note? GetByUserAndAnime(int utilisateurId, long malId);
    List<Note> GetByAnime(long malId);
    void Add(Note note);
    void Update(Note note);
}