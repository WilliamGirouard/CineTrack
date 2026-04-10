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
        Anime? GetAnimeByMalId(int id);
        Anime? GetAnimeById(int id);
        List<Anime> GetAnimes();
        void AddAnime(Anime anime);
        void UpdateAnime(Anime anime);
        void DeleteAnime(Anime anime);
    }
}
