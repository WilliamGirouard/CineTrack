using JikanDotNet;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CineTrack.Services.Interfaces
{
    public interface IAnimeApiService
    {
        Task<AnimeFull> GetAnimeByIdAsync(int id);

        Task<List<Anime>> SearchAnimeAsync(string query);
      
        Task<ICollection<Anime>> GetTrendingAnimesAsync();
    }
}
