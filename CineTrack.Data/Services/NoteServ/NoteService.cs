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
            var existing = _repository.GetByUserAndAnime(utilisateurId, malId);
            if (existing == null)
            {
                await Task.Run(() => _repository.Add(new Note
                {
                    UtilisateurId = utilisateurId,
                    MalId = malId,
                    NoteUtilisateur = note,
                    DateAdded = DateTime.UtcNow
                }));
            }
            else
            {
                existing.NoteUtilisateur = note;
                await Task.Run(() => _repository.Update(existing));
            }
        }

        public double? GetCommunityScore(long malId)
        {
            var notes = _repository.GetByAnime(malId);
            if (notes.Count == 0) return null;
            return notes.Average(r => (double)r.NoteUtilisateur);
        }

        public int? GetNote(int utilisateurId, long malId)
            => _repository.GetByUserAndAnime(utilisateurId, malId)?.NoteUtilisateur;
    }
}
