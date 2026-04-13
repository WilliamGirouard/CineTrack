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
            var entries = await _repository.GetByUtilisateurAsync(utilisateurId);
            foreach (var entry in entries)
            {
                entry.Anime = await _animeRepository.GetAnimeByIdAsync(entry.AnimeId);
            }
            return entries;
        }

        public async Task AddEntryAsync(int utilisateurId, int malId, int? note = null, string? commentaire = null)
        {
            var existing = await _repository.GetAsync(utilisateurId, animeId);
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
            var entry = await _repository.GetAsync(utilisateurId, animeId);
            if (entry == null)
                throw new Exception("Entry not found for this user-anime pair.");

            entry.Note = note;
            await _repository.UpdateAsync(entry);
        }

        public async Task UpdateCommentaireAsync(int utilisateurId, int malId, string commentaire)
        {
            var entry = await _repository.GetAsync(utilisateurId, animeId);
            if (entry == null)
                throw new Exception("Entry not found for this user-anime pair.");

            entry.Commentaire = commentaire;
            await _repository.UpdateAsync(entry);
        }

        public async Task DeleteEntryAsync(int utilisateurId, int animeId)
        {
            var entry = await _repository.GetAsync(utilisateurId, animeId);
            if (entry != null)
            {
                await _repository.DeleteAsync(entry);
            }
        }
    }
}