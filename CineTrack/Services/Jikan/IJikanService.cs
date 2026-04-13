using JikanDotNet;

namespace CineTrack.Services.Jikan;

public interface IJikanService
{
    Task<AnimeFull> GetAnimeByIdAsync(int id);
    Task<ICollection<Anime>> GetAnimesByGenreAsync(int genreId, int page = 1);
    
    Task<ICollection<Anime>> GetTrendingAnimesAsync();
}