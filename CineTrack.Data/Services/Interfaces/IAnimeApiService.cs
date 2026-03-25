using JikanDotNet;
using System.Threading.Tasks;

namespace CineTrack.Services.Interfaces
{
    public interface IAnimeApiService
    {
        Task<AnimeFull> GetAnimeByIdAsync(int id);
    }
}
