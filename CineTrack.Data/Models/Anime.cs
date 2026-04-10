using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Models
{
    public class Anime
    {
        [Key]
        public int Id { get; set; }

        public int? MalId { get; set; }

        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(200)]
        public string? TitleEnglish { get; set; }

        public string? Synopsis { get; set; }

        public string? ImageUrl { get; set; }

        [MaxLength(10)]
        public string? Season { get; set; }

        public int? Year { get; set; }
    }
}
