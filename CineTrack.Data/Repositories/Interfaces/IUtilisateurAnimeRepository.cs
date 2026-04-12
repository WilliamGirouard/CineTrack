using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTrack.Data.Models;

namespace CineTrack.Data.Repositories.Interfaces
{
    public interface IUtilisateurAnimeRepository
    {
        Task<UtilisateurAnime?> GetAsync(int utilisateurId, int animeId);
        Task<List<UtilisateurAnime>> GetByUtilisateurAsync(int utilisateurId);
        Task AddAsync(UtilisateurAnime entry);
        Task UpdateAsync(UtilisateurAnime entry);
        Task DeleteAsync(UtilisateurAnime entry);
    }
}