using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CineTrack.Data.Models.Anime
{
    internal class Image
    {
        [JsonPropertyName("image_url")] public string ImageUrl { get; set; }

        [JsonPropertyName("small_image_url")] public string SmallImageUrl { get; set; }

        [JsonPropertyName("medium_image_url")] public string MediumImageUrl { get; set; }

        [JsonPropertyName("large_image_url")] public string LargeImageUrl { get; set; }

        [JsonPropertyName("maximum_image_url")] public string MaximumImageUrl { get; set; }
    }
}
