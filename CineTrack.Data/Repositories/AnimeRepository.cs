using CineTrack.Data.Context;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Repositories
{
    public class AnimeRepository : IAnimeRepository
    {
        private readonly CineTrackDbContext _context;
        public AnimeRepository(CineTrackDbContext context)
        {
            _context = context;
        }
        public async Task AddAnimeAsync(Anime anime)
        {
            _context.Animes.Add(anime);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnimeAsync(Anime anime)
        {
            if (anime.MalId == null) return;
            var exists = await GetAnimeByMalIdAsync(anime.MalId.Value);
            if (exists != null)
            {
                Console.WriteLine(@"Anime Deleted: " + anime.Title);
                _context.Animes.Remove(anime);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Anime?> GetAnimeByMalIdAsync(int id)
        { 
            return await _context.Animes.FirstOrDefaultAsync(a => a.MalId == id);
        }

        public async Task<Anime?> GetAnimeByIdAsync(int id)
        {
            return await _context.Animes.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Anime>> GetAnimesAsync()
        {
            return await _context.Animes.ToListAsync();
        }

        public async Task UpdateAnimeAsync(Anime anime)
        {
            _context.Animes.Update(anime);
            await _context.SaveChangesAsync();
            Console.WriteLine(@"User Updated: " + anime.Title);
        }
    }
}
