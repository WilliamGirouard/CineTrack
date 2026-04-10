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
        Anime? GetAnimeByMalId(int malId);
        List<Anime> GetAnimes(); 
        void DeleteAnime(Anime anime);
        Task AddAnimeFromJikanAsync(int malId);
    }
}
