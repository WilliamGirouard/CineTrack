using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CineTrack.Data.Models.Anime
{
    internal class MalUrl
    {
        [JsonPropertyName("mal_id")] public long MalId { get; set; }

        [JsonPropertyName("type")] public string Type { get; set; }

        [JsonPropertyName("url")] public string Url { get; set; }

        [JsonPropertyName("name")] public string Name { get; set; }

        /// Overriden ToString method.
        /// </summary>
        /// <returns>Title if not null, base method elsewhere.</returns>
        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
