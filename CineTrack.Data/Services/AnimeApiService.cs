using CineTrack.Services.Interfaces;
using JikanDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Services
{
    public class AnimeApiService : IAnimeApiService
    {
        private readonly IJikan _api;

        public AnimeApiService()
        { 
            _api = new Jikan();
        }

        public async Task<AnimeFull> GetAnimeByIdAsync(int id)
        {
            var response = await _api.GetAnimeFullDataAsync(id);
            return response.Data;
        }
    }
}
