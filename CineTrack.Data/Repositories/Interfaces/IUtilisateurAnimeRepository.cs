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
        UtilisateurAnime? Get(int utilisateurId, int animeId);
        List<UtilisateurAnime> GetByUtilisateur(int utilisateurId);
        List<UtilisateurAnime> GetByMalId(int MalId);
        void Add(UtilisateurAnime entry);
        void Update(UtilisateurAnime entry);
        void Delete(UtilisateurAnime entry);
    }
}