using CineTrack.Services.Interfaces;
using JikanDotNet;

namespace CineTrack.Services.Jikan
{
    public class JikanService : IJikanService
    {
        private readonly IJikan _api;

        private readonly Dictionary<long, AnimeFull> _animeCache = new();

        public JikanService()
        {
            _api = new JikanDotNet.Jikan();
        }

        public async Task<AnimeFull> GetAnimeByIdAsync(long malId)
        {
            if (_animeCache.TryGetValue((int)malId, out var cached))
                return cached;

            var response = await _api.GetAnimeFullDataAsync(malId);

            if (response?.Data != null)
                _animeCache[malId] = response.Data;

            return response.Data;
        }

        public async Task<ICollection<Anime>> GetAnimesByGenreAsync(int genreId, int page = 1)
        {
            var result = new List<Anime>();
            int currentPage = page;

            while (result.Count < 20)
            {
                var config = new AnimeSearchConfig
                {
                    PageSize = 25,
                    Page = currentPage,
                    Genres = new List<AnimeGenreSearch> { (AnimeGenreSearch)genreId },
                    OrderBy = AnimeSearchOrderBy.Score,
                    SortDirection = SortDirection.Descending
                };

                var response = await _api.SearchAnimeAsync(config);

                // Filter client-side since Jikan ignores the Genres filter
                var filtered = response.Data
                    .Where(a => a.Genres.Any(g => g.MalId == genreId))
                    .ToList();

                result.AddRange(filtered);
                currentPage++;

                if (response.Pagination?.HasNextPage == false)
                    break;
            }

            return result.Take(20).ToList();
        }

        public async Task<ICollection<Anime>> SearchAnimeAsync(string query, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Anime>();

            var response = await _api.SearchAnimeAsync(query);

            return response.Data
                .Take(10)
                .ToList();
        }

        public async Task<ICollection<Anime>> GetTrendingAnimesAsync()
        {
            var response = await _api.GetTopAnimeAsync();
            return response.Data;


        }
    }
}