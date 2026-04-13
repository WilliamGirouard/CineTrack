using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineTrack.Data.Services
{
    public class UtilisateurAnimeService : IUtilisateurAnimeService
    {
        private readonly IUtilisateurAnimeRepository _repository;

        public UtilisateurAnimeService(IUtilisateurAnimeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UtilisateurAnime>> GetByUtilisateurAsync(int utilisateurId)
        {
            return await _repository.GetByUtilisateurAsync(utilisateurId);
        }

        public async Task AddEntryAsync(int utilisateurId, int malId, int? note = null, string? commentaire = null)
        {
            var existing = await _repository.GetAsync(utilisateurId, malId);
            if (existing != null)
                throw new Exception("Entry already exists for this user-anime pair.");

            var entry = new UtilisateurAnime
            {
                UtilisateurId = utilisateurId,
                MalId = malId,
                Note = note,
                Commentaire = commentaire,
                DateAjout = DateTime.UtcNow
            };

            await _repository.AddAsync(entry);
        }

        public async Task UpdateNoteAsync(int utilisateurId, int malId, int note)
        {
            var entry = await _repository.GetAsync(utilisateurId, malId);
            if (entry == null)
                throw new Exception("Entry not found.");

            entry.Note = note;
            await _repository.UpdateAsync(entry);
        }

        public async Task UpdateCommentaireAsync(int utilisateurId, int malId, string commentaire)
        {
            var entry = await _repository.GetAsync(utilisateurId, malId);
            if (entry == null)
                throw new Exception("Entry not found.");

            entry.Commentaire = commentaire;
            await _repository.UpdateAsync(entry);
        }

        public async Task DeleteEntryAsync(int utilisateurId, int malId)
        {
            var entry = await _repository.GetAsync(utilisateurId, malId);
            if (entry != null)
            {
                await _repository.DeleteAsync(entry);
            }
        }

        public async Task<List<UtilisateurAnime>> GetByMalIdAsync(int malId)
        {
            return await _repository.GetByMalIdAsync(malId);
        }
    }
}