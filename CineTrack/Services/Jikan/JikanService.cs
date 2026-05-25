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

        /*
         * Sources that helped/inspired
         * 1. https://github.com/Ervie/jikan.net
         * 2. https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/generate-consume-asynchronous-stream#examine-the-implementation:~:text=retrieved%20from%20GitHub.-,Examine%20the%20implementation,-The%20implementation%20reveals
         */
        public async Task<ICollection<Anime>> GetAnimesByGenreAsync(int genreId, int page = 1)
        {
            var result = new List<Anime>();
            int currentPage = page;
            bool hasMorePages = true;

            while (result.Count < 20 && hasMorePages) // Keeps searching through pages until it finds 20 animes or until it cant search anymore (2)
            {
                var response = await _api.SearchAnimeAsync( // gets animes based on config (1)
                    new AnimeSearchConfig // https://github.com/Ervie/jikan.net/blob/master/JikanDotNet/Model/Search/AnimeSearchConfig.cs
                    {
                        PageSize = 25,
                        Page = currentPage,
                        OrderBy = AnimeSearchOrderBy.Score,
                        SortDirection = SortDirection.Descending,
                        Sfw = true
                    }
                );

                // Filter client-side since Jikan genres filter doesnt work
                var filtered = response.Data
                    .Where(anime => anime.Genres.Any(genre => genre.MalId == genreId)) // We use .Any() because an anime can have multiple genres
                    .ToList();

                result.AddRange(filtered); // adds filtered results into the final result
                currentPage++;
                hasMorePages = response.Pagination?.HasNextPage ?? false; // This is a safe guard in case Jikan doesnt have anymore pages (2)
            }

            return result.Take(20).ToList(); // Makes sure we actually get 20 animes at most
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