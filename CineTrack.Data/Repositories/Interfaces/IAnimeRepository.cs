using CineTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Repositories.Interfaces
{
    public interface IAnimeRepository
    {
        Task<Anime?> GetAnimeByMalIdAsync(int id);
        Task<Anime?> GetAnimeByIdAsync(int id);
        Task<List<Anime>> GetAnimesAsync();
        Task AddAnimeAsync(Anime anime);
        Task UpdateAnimeAsync(Anime anime);
        Task DeleteAnimeAsync(Anime anime);
    }
}
