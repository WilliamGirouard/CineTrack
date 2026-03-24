using JikanDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CineTrack.Data.Models.Anime
{
    public class Anime
    {
        // Données géré par l'API
        [JsonPropertyName("mal_id")] public int MalId { get; set; }
        [JsonPropertyName("title_english")] public string? Titre { get; set; }
        [JsonPropertyName("synopsis")] public string? Description { get; set; }
        [JsonPropertyName("year")] public int? AnneeDePublication { get; set; }
        [JsonPropertyName("episodes")] public int? NbEpisodes { get; set; }
        [JsonPropertyName("type")] public string? TypeDeMedia { get; set; }
        [JsonPropertyName("duration")] public string? DureeParEpisode { get; set; }
        [JsonPropertyName("images")] public ImagesSet? Images { get; set; }
        [JsonPropertyName("genres")] public ICollection<MalUrl>? Genres { get; set; }

        // Nos Données
        public float Note { get; set; }
        public List<Object> Commentaires { get; set; }
    }
}
