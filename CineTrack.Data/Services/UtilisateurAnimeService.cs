using CineTrack.Data.Models;
using CineTrack.Data.Repositories;
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
        private readonly IAnimeRepository _animeRepository;

        public UtilisateurAnimeService(IUtilisateurAnimeRepository repository, IAnimeRepository animeRepository)
        {
            _repository = repository;
            _animeRepository = animeRepository;
        }

        public List<UtilisateurAnime> GetByUtilisateur(int utilisateurId)
        {
            var entries = _repository.GetByUtilisateur(utilisateurId);
            foreach (var entry in entries)
            {
                entry.Anime = _animeRepository.GetAnimeById(entry.AnimeId);
            }
            return entries;
        }

        public async Task AddEntryAsync(int utilisateurId, int animeId, int? note = null, string? commentaire = null)
        {
            var existing = _repository.Get(utilisateurId, animeId);
            if (existing != null)
                throw new Exception("Entry already exists for this user-anime pair.");

            var entry = new UtilisateurAnime
            {
                UtilisateurId = utilisateurId,
                AnimeId = animeId,
                Note = note,
                Commentaire = commentaire,
                DateAjout = DateTime.UtcNow
            };

            await Task.Run(() => _repository.Add(entry));
        }

        public async Task UpdateNoteAsync(int utilisateurId, int animeId, int note)
        {
            var entry = _repository.Get(utilisateurId, animeId);
            if (entry == null)
                throw new Exception("Entry not found for this user-anime pair.");

            entry.Note = note;
            await Task.Run(() => _repository.Update(entry));
        }

        public async Task UpdateCommentaireAsync(int utilisateurId, int animeId, string commentaire)
        {
            var entry = _repository.Get(utilisateurId, animeId);
            if (entry == null)
                throw new Exception("Entry not found for this user-anime pair.");

            entry.Commentaire = commentaire;
            await Task.Run(() => _repository.Update(entry));
        }

        public void DeleteEntry(int utilisateurId, int animeId)
        {
            var entry = _repository.Get(utilisateurId, animeId);
            if (entry != null)
            {
                _repository.Delete(entry);
            }
        }
    }
}