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
    public class AnimeRepository : IAnimeRepository
    {
        private readonly CineTrackDbContext _context;
        public AnimeRepository(CineTrackDbContext context)
        {
            _context = context;
        }
        public void AddAnime(Anime anime)
        {
            _context.Animes.Add(anime);
            _context.SaveChanges();
        }

        public void DeleteAnime(Anime anime)
        {
            if (GetAnimeByMalId((int)anime.MalId) != null)
            {
                Console.WriteLine(@"Anime Deleted: " + anime.Title);
                _context.Animes.Remove(anime);
                _context.SaveChanges();
            }
        }

        public Anime? GetAnimeByMalId(int id)
        {
            return _context.Animes.FirstOrDefault(a => a.MalId == id);
        }

        public Anime? GetAnimeById(int id)
        {
            return _context.Animes.FirstOrDefault(a => a.Id == id);
        }

        public List<Anime> GetAnimes()
        {
            return _context.Animes.ToList();
        }

        public void UpdateAnime(Anime anime)
        {
            _context.Animes.Update(anime);
            _context.SaveChanges();
            Console.WriteLine(@"User Updated: " + anime.Title);
        }
    }
}
