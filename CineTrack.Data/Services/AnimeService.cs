using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services.Interfaces;
using CineTrack.Services.Interfaces;
using JikanDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Services
{
    public class AnimeService : IAnimeService
    {

        private readonly IAnimeRepository _animeRepository;
        private readonly IAnimeApiService _apiService;

        public AnimeService(IAnimeRepository animeRepository, IAnimeApiService apiService)
        {
            _animeRepository = animeRepository;
            _apiService = apiService;
        }

        public async Task AddAnimeFromJikanAsync(int malId)
        {
            if (await _animeRepository.GetAnimeByMalIdAsync(malId) != null)
                throw new Exception("Anime already exists");

            var apiAnime = await _apiService.GetAnimeByIdAsync(malId);

            var anime = new Models.Anime
            {
                MalId = (int)apiAnime.MalId,
                Title = apiAnime.Title,
                TitleEnglish = apiAnime.TitleEnglish,
                Synopsis = apiAnime.Synopsis,
                ImageUrl = apiAnime.Images?.JPG?.ImageUrl,
                Season = apiAnime.Season?.ToString(),
                Year = apiAnime.Year
            };

            await _animeRepository.AddAnimeAsync(anime);
        }

        public async Task DeleteAnimeAsync(Models.Anime anime)
        {
            await _animeRepository.DeleteAnimeAsync(anime);
        }

        public async Task<Models.Anime?> GetAnimeByMalIdAsync(int malId)
        {
            return await _animeRepository.GetAnimeByMalIdAsync(malId);
        }

        public async Task<List<Models.Anime>> GetAnimesAsync()
        {
            return await _animeRepository.GetAnimesAsync();
        }
    }
}