using CineTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Services.Interfaces
{
    public interface IAnimeService
    {
        Task<Anime?> GetAnimeByMalIdAsync(int malId);
        Task<List<Anime>> GetAnimesAsync(); 
        Task DeleteAnimeAsync(Anime anime);
        Task AddAnimeFromJikanAsync(int malId);
    }
}
