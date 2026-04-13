using CineTrack.Services.Interfaces;
using JikanDotNet;

namespace CineTrack.Services.Jikan
{
    public class JikanService : IJikanService
    {
        private readonly IJikan _api;

        public JikanService()
        {
            _api = new JikanDotNet.Jikan();
        }

        public async Task<AnimeFull> GetAnimeByIdAsync(int id)
        {
            var response = await _api.GetAnimeFullDataAsync(id);
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
                result.AddRange(response.Data);
                currentPage++;

                // Stop if Jikan has no more pages
                if (response.Pagination?.HasNextPage == false)
                    break;
            }

            return result.Take(20).ToList();
        }

        public async Task<ICollection<Anime>> GetTrendingAnimesAsync()
        {
            var response = await _api.GetTopAnimeAsync();
            return response.Data;


        }
    }
}