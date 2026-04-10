using CineTrack.Data.Context;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Repositories
{
    public class UtilisateurAnimeRepository : IUtilisateurAnimeRepository
    {
        private readonly CineTrackDbContext _context;

        public UtilisateurAnimeRepository(CineTrackDbContext context)
        {
            _context = context;
        }

        public void Add(UtilisateurAnime entry)
        {
            _context.UtilisateurAnimes.Add(entry);
            _context.SaveChanges();
        }

        public void Delete(UtilisateurAnime entry)
        {
            _context.UtilisateurAnimes.Remove(entry);
            _context.SaveChanges();
        }

        public UtilisateurAnime? Get(int utilisateurId, int animeId)
        {
            return _context.UtilisateurAnimes.FirstOrDefault(ua => ua.UtilisateurId == utilisateurId && ua.AnimeId == animeId);
        }

        public List<UtilisateurAnime> GetByUtilisateur(int utilisateurId)
        {
            return _context.UtilisateurAnimes.Where(ua => ua.UtilisateurId == utilisateurId).ToList();
        }

        public void Update(UtilisateurAnime entry)
        {
            _context.UtilisateurAnimes.Update(entry);
            _context.SaveChanges();
        }
    }
}
