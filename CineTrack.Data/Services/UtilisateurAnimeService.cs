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

        public List<UtilisateurAnime> GetByUtilisateur(int utilisateurId)
        {
            return _repository.GetByUtilisateur(utilisateurId);
        }

        public List<UtilisateurAnime> GetByMalId(int malId)
        {
            return _repository.GetByMalId(malId);
        }

        public async Task AddEntryAsync(int utilisateurId, int malId, int? note = null, string? commentaire = null)
        {
            var existing = _repository.Get(utilisateurId, malId);
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

            await Task.Run(() => _repository.Add(entry));
        }

        public async Task UpdateNoteAsync(int utilisateurId, int malId, int note)
        {
            var entry = _repository.Get(utilisateurId, malId);
            if (entry == null)
                throw new Exception("Entry not found for this user-anime pair.");

            entry.Note = note;
            await Task.Run(() => _repository.Update(entry));
        }

        public async Task UpdateCommentaireAsync(int utilisateurId, int malId, string commentaire)
        {
            var entry = _repository.Get(utilisateurId, malId);
            if (entry == null)
                throw new Exception("Entry not found for this user-anime pair.");

            entry.Commentaire = commentaire;
            await Task.Run(() => _repository.Update(entry));
        }

        public void DeleteEntry(int utilisateurId, int malId)
        {
            var entry = _repository.Get(utilisateurId, malId);
            if (entry != null)
            {
                _repository.Delete(entry);
            }
        }
    }
}