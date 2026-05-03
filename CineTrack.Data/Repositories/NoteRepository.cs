using CineTrack.Data.Context;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineTrack.Data.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly IDbContextFactory<CineTrackDbContext> _context;

        public NoteRepository(IDbContextFactory<CineTrackDbContext> context)
        {
            _context = context;
        }

        public async Task<Note?> GetNoteByUserIdAndAnimeIdAsync(int utilisateurId, long malId)
        {
            using var context = _context.CreateDbContext();
            return await context.Notes
                .FirstOrDefaultAsync(r => r.UtilisateurId == utilisateurId && r.MalId == malId);
        }

        public async Task<List<Note>> GetNotesByAnimeIdAsync(long malId)
        {
            using var context = _context.CreateDbContext();
            return await context.Notes
                .Where(r => r.MalId == malId)
                .ToListAsync();
        }

        public async Task<List<Note>> GetNotesByUserIdAsync(int utilisateurId)
        {
            using var context = _context.CreateDbContext();
            return await context.Notes
                .Where(n => n.UtilisateurId == utilisateurId)
                .OrderByDescending(n => n.DateAdded)
                .ToListAsync();
        }

        public async Task AddNoteAsync(Note note)
        {
            using var context = _context.CreateDbContext();
            await context.Notes.AddAsync(note);
            await context.SaveChangesAsync();
        }

        public async Task UpdateNoteAsync(Note note)
        {
            using var context = _context.CreateDbContext();
            context.Notes.Update(note);
            await context.SaveChangesAsync();
        }

        public async Task<Note?> GetNoteByIdAsync(int noteId)
        {
            using var context = _context.CreateDbContext();
            return await context.Notes.FindAsync(noteId);
        }

        public async Task DeleteNoteByIdAsync(int noteId)
        {
            using var context = _context.CreateDbContext();
            var note = await context.Notes.FindAsync(noteId);
            if (note != null)
            {
                context.Notes.Remove(note);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Note>> GetNotesRecentesAsync()
        {
            using var context = _context.CreateDbContext();
            return await context.Notes
                .OrderByDescending(n => n.DateAdded)
                .Take(10)
                .ToListAsync();
        }
    }
}
