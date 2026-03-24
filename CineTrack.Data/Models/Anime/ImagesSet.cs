using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CineTrack.Data.Models.Anime
{
    internal class ImagesSet
    {
        [JsonPropertyName("jpg")]
        public Image JPG { get; set; }

        [JsonPropertyName("webp")]
        public Image WebP { get; set; }
    }
}
