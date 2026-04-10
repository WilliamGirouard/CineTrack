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
            if (_animeRepository.GetAnimeByMalId(malId) != null)
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

            _animeRepository.AddAnime(anime);
        }

        public void DeleteAnime(Models.Anime anime)
        {
            _animeRepository.DeleteAnime(anime);
        }

        public Models.Anime? GetAnimeByMalId(int malId)
        {
            return _animeRepository.GetAnimeByMalId(malId);
        }

        public List<Models.Anime> GetAnimes()
        {
            return _animeRepository.GetAnimes();
        }
    }
}