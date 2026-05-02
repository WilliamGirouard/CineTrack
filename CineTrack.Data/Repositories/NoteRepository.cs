using CineTrack.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;

namespace CineTrack.Data.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly CineTrackDbContext _context;

        public NoteRepository(CineTrackDbContext context)
        {
            _context = context;
        }

        public Note? GetByUserAndAnime(int utilisateurId, long malId)
            => _context.Notes.FirstOrDefault(r =>
                r.UtilisateurId == utilisateurId && r.MalId == malId);

        public List<Note> GetByAnime(long malId)
            => _context.Notes.Where(r => r.MalId == malId).ToList();

        public void Add(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        public void Update(Note note)
        {
            _context.Notes.Update(note);
            _context.SaveChanges();
        }
    }
}
