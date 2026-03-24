using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using JikanDotNet;

namespace CineTrack.Services
{
    internal class AnimeApiService
    {
        Random rand = new Random();
        private readonly IJikan _api = new Jikan();

        public void test()
        {
            var randomAnime = _api.GetAnimeAsync(rand.NextInt64(1, 20));
            Console.WriteLine(randomAnime);
        }
    }
}
