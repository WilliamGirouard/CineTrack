using CineTrack.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineTrack.Data.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly CineTrackDbContext _context;

        public NoteRepository(CineTrackDbContext context)
        {
            _context = context;
        }

        public async Task<Note?> GetNoteByUserIdAndAnimeIdAsync(int utilisateurId, long malId)
        {
            return await _context.Notes
                .FirstAsync(r => r.UtilisateurId == utilisateurId && r.MalId == malId);
        }

        public async Task<List<Note>> GetNotesByAnimeIdAsync(long malId)
        {
            return await _context.Notes
                .Where(r => r.MalId == malId)
                .ToListAsync();
        }
            

        public async Task AddNoteAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNoteAsync(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }
        public async Task<Note?> GetNoteByIdAsync(int noteId)
        {
            return await _context.Notes.FindAsync(noteId);
        }
        public async Task DeleteNoteByIdAsync(int noteId)
        {
            var note = await _context.Notes.FindAsync(noteId);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Note>> GetNotesRecentesAsync()
        {
            return await _context.Notes
                .OrderByDescending(n => n.DateAdded)
                .Take(10)
                .ToListAsync();
        }
    }
}
