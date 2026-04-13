using CineTrack.Data.Context;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CineTrack.Data.Repositories
{
    public class UtilisateurAnimeRepository : IUtilisateurAnimeRepository
    {
        private readonly CineTrackDbContext _context;

        public UtilisateurAnimeRepository(CineTrackDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UtilisateurAnime entry)
        {
            _context.UtilisateurAnimes.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UtilisateurAnime entry)
        {
            _context.UtilisateurAnimes.Remove(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<UtilisateurAnime?> GetAsync(int utilisateurId, int animeId)
        {
            return await _context.UtilisateurAnimes.FirstOrDefaultAsync(ua => ua.UtilisateurId == utilisateurId && ua.AnimeId == animeId);
        }

        public async Task<List<UtilisateurAnime>> GetByUtilisateurAsync(int utilisateurId)
        {
          return await _context.UtilisateurAnimes
            .Where(ua => ua.UtilisateurId == utilisateurId)
            .Include(ua => ua.Utilisateur)
            .ToListAsync();
        }

        public async Task<List<UtilisateurAnime>> GetByMalIdAsync(int malId)
        {
            return await _context.UtilisateurAnimes
              .Where(ua => ua.MalId == malId)
              .Include(ua => ua.Utilisateur)
              .ToListAsync();
        }

        public async Task UpdateAsync(UtilisateurAnime entry)
        {
            _context.UtilisateurAnimes.Update(entry);
            await _context.SaveChangesAsync();
        }
    }
}