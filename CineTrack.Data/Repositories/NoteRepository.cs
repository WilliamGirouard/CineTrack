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

        public async Task<Note?> GetNoteByUserAndAnime(int utilisateurId, long malId)
            => _context.Notes.FirstOrDefault(r =>
                r.UtilisateurId == utilisateurId && r.MalId == malId);

        public async Task<List<Note>> GetNotesByAnimeAsync(long malId)
            => _context.Notes.Where(r => r.MalId == malId).ToList();

        public async Task AddNoteAsync(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        public async Task UpdateNoteAsync(Note note)
        {
            _context.Notes.Update(note);
            _context.SaveChanges();
        }
    }
}
