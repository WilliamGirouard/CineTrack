using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Models;

namespace CineTrack.Data.Services.NoteServ
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _repository;

        public NoteService(INoteRepository repository)
        {
            _repository = repository;
        }

        public async Task RateAsync(int utilisateurId, long malId, int note)
        {
            var existing = await _repository.GetNoteByUserIdAndAnimeIdAsync(utilisateurId, malId);
            if (existing == null)
            {
                await _repository.AddNoteAsync(new Note
                {
                    UtilisateurId = utilisateurId,
                    MalId = malId,
                    NoteUtilisateur = note,
                    DateAdded = DateTime.UtcNow
                });
            }
            else
            {
                existing.NoteUtilisateur = note;
                await _repository.UpdateNoteAsync(existing);
            }
        }

        public async Task<double?> GetCommunityScoreAsync(long malId)
        {
            var notes = await _repository.GetNotesByAnimeIdAsync(malId);
            if (notes.Count == 0) return null;
            return notes.Average(r => (double)r.NoteUtilisateur);
        }

        public async Task<int?> GetNoteAsync(int utilisateurId, long malId)
        {
            var note = await _repository.GetNoteByUserIdAndAnimeIdAsync(utilisateurId, malId);
            return note?.NoteUtilisateur;
        }
    }
}
